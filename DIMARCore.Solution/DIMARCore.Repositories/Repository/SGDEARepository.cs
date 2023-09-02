using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class SGDEARepository : GenericRepository<SGDEA_PREVISTAS>
    {

        public async Task<IEnumerable<RadicadoDTO>> GetRadicadosTitulosByCedula(string cedula)
        {
            var query = (from titulo in _context.GENTEMAR_TITULOS
                         join usuario in _context.GENTEMAR_DATOSBASICOS on titulo.id_gentemar equals usuario.id_gentemar
                         where usuario.documento_identificacion.Equals(cedula)
                         select titulo.radicado
                         ).ToList();

            List<RadicadoDTO> radicados = new List<RadicadoDTO>();
            radicados = await (from listado in _context.TABLA_SGDEA_PREVISTAS
                               where listado.numero_identificacion_usuario.Equals(cedula) && listado.tipo_tramite.Contains(Constantes.TRAMITE_TITULOS)
                               && listado.estado.Equals(Constantes.ESTADOTRAMITESGDA)
                               && !query.Contains(listado.radicado.ToString())
                               group listado by new { listado.radicado, listado.tipo_tramite } into grouped
                               where grouped.Count() == 1
                               select new RadicadoDTO
                               {
                                   Radicado = grouped.Key.radicado,
                                   Conteo = grouped.Count(),
                                   TipoTramite = grouped.Key.tipo_tramite
                               }).ToListAsync();


            return radicados;
        }


        public async Task<IEnumerable<RadicadoDTO>> GetRadicadosLicenciaByCedula(string cedula)
        {
            var query = (from licencia in _context.GENTEMAR_LICENCIAS
                         join usuario in _context.GENTEMAR_DATOSBASICOS on licencia.id_gentemar equals usuario.id_gentemar
                         where usuario.documento_identificacion.Equals(cedula)
                         select licencia.radicado
                         ).ToList();

            List<RadicadoDTO> radicados = new List<RadicadoDTO>();
            radicados = await (from listado in _context.TABLA_SGDEA_PREVISTAS
                               where listado.numero_identificacion_usuario.Equals(cedula) && listado.tipo_tramite.Contains(Constantes.TRAMITE_LICENCIA)
                               && listado.estado.Equals(Constantes.ESTADOTRAMITESGDA)
                               && !query.Contains(listado.radicado)
                               group listado by new { listado.radicado, listado.tipo_tramite } into grouped
                               where grouped.Count() == 1
                               select new RadicadoDTO
                               {
                                   Radicado = grouped.Key.radicado,
                                   Conteo = grouped.Count(),
                                   TipoTramite = grouped.Key.tipo_tramite
                               }).ToListAsync();


            return radicados;
        }

        public async Task<(decimal? Radicado, DateTime? Fecha)> FechaRadicado(string numero_sgdea)
        {
            decimal Radicado = Convert.ToDecimal(numero_sgdea);
            var data = await _context.TABLA_SGDEA_PREVISTAS.Where(x => x.radicado == Radicado && x.estado.Equals(Constantes.PREVISTATRAMITE))
                 .Select(y => new { Radicado = y.radicado, Fecha = y.fecha_estado }).FirstOrDefaultAsync();

            if (data == null)
                return (null, null);

            return (data.Radicado, data.Fecha);

        }

        public async Task<IEnumerable<RadicadoInfoDTO>> GetRadicadosInfoPersonaParaEstupefacientes(bool isTitulo)
        {
            IList<RadicadoInfoDTO> data = new List<RadicadoInfoDTO>();
            var radicadosUsados = await GetRadicadosEstupefacientesEnUso();

            var filtroTituloLicencia = isTitulo ? Constantes.TRAMITE_TITULOS : Constantes.TRAMITE_LICENCIA;

            data = await (from sgdea in _context.TABLA_SGDEA_PREVISTAS
                          where sgdea.tipo_tramite.Contains(filtroTituloLicencia)
                          && sgdea.estado.Equals(Constantes.ESTADOTRAMITESGDA)
                          && !radicadosUsados.Contains(sgdea.radicado.ToString())
                          group sgdea by new { sgdea.radicado, cedula = sgdea.numero_identificacion_usuario } into grouped
                          where grouped.Count() == 1
                          select new RadicadoInfoDTO
                          {

                              Radicado = grouped.Key.radicado.ToString(),
                              Conteo = grouped.Count(),
                              FechaRadicado = grouped.FirstOrDefault().fecha_estado,
                              NumeroIdentificacionSGDEA = grouped.Key.cedula,
                              Info = (from usuario in _context.GENTEMAR_ANTECEDENTES_DATOSBASICOS
                                      join documento in _context.APLICACIONES_TIPO_DOCUMENTO on usuario.id_tipo_documento equals documento.ID_TIPO_DOCUMENTO
                                      where usuario.identificacion.Equals(grouped.Key.cedula)

                                      select new InfoPersonaDTO
                                      {
                                          Identificacion = usuario.identificacion,
                                          Nombres = usuario.nombres,
                                          Apellidos = usuario.apellidos,
                                          FechaNacimiento = usuario.fecha_nacimiento,
                                          TipoDocumento = documento.SIGLA,
                                          TipoDocumentoId = documento.ID_TIPO_DOCUMENTO,
                                      }).FirstOrDefault()
                          }).ToListAsync();
            return data;
        }

        private async Task<IList<string>> GetRadicadosEstupefacientesEnUso()
        {
            return await (from antecedente in _context.GENTEMAR_ANTECEDENTES
                          join usuario in _context.GENTEMAR_ANTECEDENTES_DATOSBASICOS on antecedente.id_gentemar_antecedente
                          equals usuario.id_gentemar_antecedente
                          select antecedente.numero_sgdea).ToListAsync();
        }

        public async Task CreateSgdaPrevista(IEnumerable<SGDEA_PREVISTAS> previstas)
        {
            using (_context)
            {
                using (var trassaction = _context.Database.BeginTransaction())
                {

                    try
                    {
                        _context.TABLA_SGDEA_PREVISTAS.AddRange(previstas);
                        await SaveAllAsync();
                        trassaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        trassaction.Rollback();
                        throw new HttpStatusCodeException(Responses.SetInternalServerErrorResponse(ex, "Error al Guardar la prevista"));
                    }
                }
            }
        }
    }
}

using Dapper;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using GenteMarCore.Entities;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class SGDEARepository : GenericRepository<SGDEA_PREVISTAS>
    {
        private readonly CoreDapperContext _coreContextDapper;
        public SGDEARepository()
        {
            _coreContextDapper = new CoreDapperContext();
        }
        public async Task<IEnumerable<RadicadoDTO>> GetRadicadosTitulosByCedula(string cedula)
        {
            var radicadosEnUso = await (from titulo in _context.GENTEMAR_TITULOS
                                        join usuario in _context.GENTEMAR_DATOSBASICOS on titulo.id_gentemar equals usuario.id_gentemar
                                        where usuario.documento_identificacion.Equals(cedula)
                                        select titulo.radicado).ToListAsync();

            List<RadicadoDTO> radicados = new List<RadicadoDTO>();
            var query = (from listado in _context.TABLA_SGDEA_PREVISTAS
                         where listado.numero_identificacion_usuario.Equals(cedula)
                         && listado.tipo_tramite.Contains(Constantes.TRAMITE_TITULOS)
                         && !radicadosEnUso.Contains(listado.radicado.ToString())
                         group listado by new { listado.radicado, listado.tipo_tramite } into grouped
                         select new RadicadoDTO
                         {
                             Radicado = grouped.Key.radicado,
                             Conteo = grouped.Count(),
                             TipoTramite = grouped.Key.tipo_tramite
                         });

            radicados = await query.Where(x => x.Conteo == 1).AsNoTracking().ToListAsync();
            return radicados;
        }


        public async Task<IEnumerable<RadicadoDTO>> GetRadicadosLicenciaByCedula(string cedula)
        {
            List<RadicadoDTO> radicados = new List<RadicadoDTO>();
            var radicadosEnUso = await (from licencia in _context.GENTEMAR_LICENCIAS
                                        join usuario in _context.GENTEMAR_DATOSBASICOS on licencia.id_gentemar equals usuario.id_gentemar
                                        where usuario.documento_identificacion.Equals(cedula)
                                        select licencia.radicado).ToListAsync();
            var query = (from listado in _context.TABLA_SGDEA_PREVISTAS
                         where listado.numero_identificacion_usuario.Equals(cedula)
                         && listado.tipo_tramite.Contains(Constantes.TRAMITE_LICENCIA)
                         && !radicadosEnUso.Contains(listado.radicado)
                         group listado by new { listado.radicado, listado.tipo_tramite } into grouped
                         select new RadicadoDTO
                         {
                             Radicado = grouped.Key.radicado,
                             Conteo = grouped.Count(),
                             TipoTramite = grouped.Key.tipo_tramite
                         });

            radicados = await query.Where(x => x.Conteo == 1).AsNoTracking().ToListAsync();
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

        public async Task<IEnumerable<RadicadoInfoDTO>> GetRadicadosInfoPersonaParaEstupefacientes(RadicadoSGDEAFilter filter,
            CancellationTokenSource tokenSource)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(filter.Identificacion))
                {
                    bool isExisteInGenteDeMar = await _context.GENTEMAR_DATOSBASICOS.AnyAsync(x => x.documento_identificacion.Equals(filter.Identificacion));
                    return await GetRadicadosInfoPersonaParaEstupefacientesPorIdentificacion(filter, isExisteInGenteDeMar, tokenSource);
                }
                else
                {
                    return await GetRadicadosInfoPersonaParaEstupefacientesDapper(filter, tokenSource);
                }

            }
            catch (OperationCanceledException ex)
            {
                // Handle the cancellation
                // Aquí puedes realizar las acciones necesarias si se ha solicitado la cancelación
                throw new HttpStatusCodeException(Responses.SetRequestCanceledResponse(ex, "La operación ha sido cancelada, GetRadicadosInfoPersonaParaEstupefacientes"));
            }
            catch (Exception ex)
            {
                throw new HttpStatusCodeException(System.Net.HttpStatusCode.InternalServerError, ex);
            }
            finally
            {
                _coreContextDapper.CloseConnection();
            }
        }

        private async Task<IEnumerable<RadicadoInfoDTO>> GetRadicadosInfoPersonaParaEstupefacientesPorIdentificacion(RadicadoSGDEAFilter filter,
            bool isExistInGenteDeMar, CancellationTokenSource tokenSource)
        {
            IList<RadicadoInfoDTO> radicados = new List<RadicadoInfoDTO>();
            var filtroTituloLicencia = filter.IsTituloNavegacion ? Constantes.TRAMITE_TITULOS : Constantes.TRAMITE_LICENCIA;
            var query = (from sgdea in _context.TABLA_SGDEA_PREVISTAS
                         where sgdea.tipo_tramite.Contains(filtroTituloLicencia)
                         && sgdea.numero_identificacion_usuario.Equals(filter.Identificacion)
                         && !_context.GENTEMAR_ANTECEDENTES.Any(antecedente => antecedente.numero_sgdea == sgdea.radicado.ToString())
                         group sgdea by new { sgdea.radicado, cedula = sgdea.numero_identificacion_usuario } into grouped
                         select new RadicadoInfoDTO
                         {
                             Radicado = grouped.Key.radicado.ToString(),
                             Conteo = grouped.Count(),
                             FechaRadicado = grouped.FirstOrDefault().fecha_estado,
                             NumeroIdentificacionSGDEA = grouped.Key.cedula,
                         });

            radicados = await query.Where(x => x.Conteo == 1).AsNoTracking().ToListAsync(tokenSource.Token);

            if (isExistInGenteDeMar)
            {
                var dataGenteDeMar = await (from gente in _context.GENTEMAR_DATOSBASICOS
                                            join documento in _context.APLICACIONES_TIPO_DOCUMENTO on gente.id_tipo_documento equals documento.ID_TIPO_DOCUMENTO
                                            where gente.documento_identificacion.Equals(filter.Identificacion)
                                            select new
                                            {
                                                gente.apellidos,
                                                gente.nombres,
                                                gente.documento_identificacion,
                                                gente.fecha_nacimiento,
                                                documento.ID_TIPO_DOCUMENTO,
                                                documento.DESCRIPCION
                                            }).FirstOrDefaultAsync(tokenSource.Token);

                foreach (var item in radicados)
                {
                    item.Nombres = dataGenteDeMar.nombres;
                    item.Apellidos = dataGenteDeMar.apellidos;
                    item.FechaNacimiento = dataGenteDeMar.fecha_nacimiento;
                    item.TipoDocumento = dataGenteDeMar.DESCRIPCION;
                    item.TipoDocumentoId = dataGenteDeMar.ID_TIPO_DOCUMENTO;
                    item.Identificacion = dataGenteDeMar.documento_identificacion;
                }
            }
            else
            {
                var dataGenteDeMarInVCITE = await (from gente in _context.GENTEMAR_ANTECEDENTES_DATOSBASICOS
                                                   join documento in _context.APLICACIONES_TIPO_DOCUMENTO on gente.id_tipo_documento equals documento.ID_TIPO_DOCUMENTO
                                                   where gente.identificacion.Equals(filter.Identificacion)
                                                   select new
                                                   {
                                                       gente.apellidos,
                                                       gente.nombres,
                                                       gente.identificacion,
                                                       gente.fecha_nacimiento,
                                                       documento.ID_TIPO_DOCUMENTO,
                                                       documento.DESCRIPCION
                                                   }).FirstOrDefaultAsync(tokenSource.Token);
                if (dataGenteDeMarInVCITE != null)
                {
                    foreach (var item in radicados)
                    {
                        item.Nombres = dataGenteDeMarInVCITE.nombres;
                        item.Apellidos = dataGenteDeMarInVCITE.apellidos;
                        item.FechaNacimiento = dataGenteDeMarInVCITE.fecha_nacimiento;
                        item.TipoDocumento = dataGenteDeMarInVCITE.DESCRIPCION;
                        item.TipoDocumentoId = dataGenteDeMarInVCITE.ID_TIPO_DOCUMENTO;
                        item.Identificacion = dataGenteDeMarInVCITE.identificacion;
                    }
                }
            }
            if (tokenSource.Token.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }
            return radicados;
        }

        private async Task<IEnumerable<RadicadoInfoDTO>> GetRadicadosInfoPersonaParaEstupefacientesDapper(RadicadoSGDEAFilter filter,
            CancellationTokenSource tokenSource)
        {
            using (IDbConnection db = _coreContextDapper.Context)
            {
                string filtroTituloLicencia = filter.IsTituloNavegacion ? $"{Constantes.TRAMITE_TITULOS}%" : $"{Constantes.TRAMITE_LICENCIA}%";
                string estado = Constantes.ESTADOTRAMITESGDA;
                string sqlQuery = $@"SELECT sgdea.radicado                      AS Radicado,
                                            1                                   AS Conteo,
                                            MAX(sgdea.fecha_estado)             AS FechaRadicado,
                                            sgdea.numero_identificacion_usuario AS NumeroIdentificacionSGDEA,
                                            MAX(usuario.identificacion)         AS Identificacion,
                                            MAX(usuario.nombres)                AS Nombres,
                                            MAX(usuario.apellidos)              AS Apellidos,
                                            MAX(usuario.fecha_nacimiento)       AS FechaNacimiento,
                                            MAX(documento.SIGLA)                AS TipoDocumento,
                                            MAX(documento.ID_TIPO_DOCUMENTO)    AS TipoDocumentoId
                                     FROM DBA.SGDEA_PREVISTAS sgdea
                                              LEFT JOIN DBA.GENTEMAR_ANTECEDENTES_DATOSBASICOS usuario
                                                        ON sgdea.numero_identificacion_usuario = usuario.identificacion
                                              LEFT JOIN DBA.APLICACIONES_TIPO_DOCUMENTO documento ON usuario.id_tipo_documento = documento.ID_TIPO_DOCUMENTO
                                     WHERE sgdea.tipo_tramite LIKE ?
                                       AND sgdea.estado = ?
                                       AND NOT EXISTS (SELECT 1
                                                       FROM DBA.GENTEMAR_ANTECEDENTES antecedente
                                                       WHERE antecedente.numero_sgdea = CAST(sgdea.radicado AS VARCHAR))
                                     GROUP BY sgdea.radicado,
                                              sgdea.numero_identificacion_usuario";


                var data = await db.QueryAsync<RadicadoInfoDTO>(
                   new CommandDefinition(sqlQuery,
                   new { filtroTituloLicencia, estado }, cancellationToken: tokenSource.Token));

                if (tokenSource.Token.IsCancellationRequested)
                {
                    throw new OperationCanceledException();
                }
                return data;
            }
        }

        public async Task CreateSgdaPrevista(IEnumerable<SGDEA_PREVISTAS> previstas)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.TABLA_SGDEA_PREVISTAS.AddRange(previstas);
                    await SaveAllAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new HttpStatusCodeException(Responses.SetInternalServerErrorResponse(ex, "Error al Guardar la prevista"));
                }
            }
        }
    }
}

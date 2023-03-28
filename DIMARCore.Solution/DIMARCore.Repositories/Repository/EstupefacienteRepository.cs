using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class EstupefacienteRepository : GenericRepository<GENTEMAR_ANTECEDENTES>
    {
        public IQueryable<ListadoEstupefacientesDTO> GetAntecedentesByFiltro(EstupefacientesFilter filtro)
        {
            var query = (from antecedente in _context.GENTEMAR_ANTECEDENTES
                         join usuario in _context.GENTEMAR_ANTECEDENTES_DATOSBASICOS on antecedente.id_gentemar_antecedente equals usuario.id_gentemar_antecedente
                         join estado in _context.GENTEMAR_ESTADO_ANTECEDENTES on antecedente.id_estado_antecedente equals estado.id_estado_antecedente
                         join tramite in _context.GENTEMAR_TIPO_TRAMITE on antecedente.id_tipo_tramite equals tramite.id_tipo_tramite
                         join capitaniaFirma in _context.APLICACIONES_CAPITANIAS on antecedente.id_capitania equals capitaniaFirma.ID_CAPITANIA
                         select new
                         {
                             antecedente,
                             usuario,
                             tramite,
                             estado,
                             capitaniaFirma
                         });

            if (!string.IsNullOrWhiteSpace(filtro.IdentificacionConPuntos))
            {
                query = query.Where(x => x.usuario.identificacion.Equals(filtro.IdentificacionConPuntos));
            }

            if (filtro.FechaInicial != null && filtro.FechaFinal != null)
            {
                filtro.FechaInicial = new DateTime(filtro.FechaInicial.Value.Year, filtro.FechaInicial.Value.Month, filtro.FechaInicial.Value.Day, 0, 0, 0, 0);
                filtro.FechaFinal = new DateTime(filtro.FechaFinal.Value.Year, filtro.FechaFinal.Value.Month, filtro.FechaFinal.Value.Day, 0, 0, 0, 0);
                filtro.FechaFinal = filtro.FechaFinal.Value.AddHours(24).AddSeconds(-1);
                query = query.Where(x => x.antecedente.fecha_solicitud_sede_central >= filtro.FechaInicial && x.antecedente.fecha_solicitud_sede_central <= filtro.FechaFinal);
            }

            if (filtro.EstadoId > 0)
            {
                query = query.Where(x => x.antecedente.id_estado_antecedente == filtro.EstadoId);
            }
            if (filtro.TramiteId > 0)
            {
                query = query.Where(x => x.antecedente.id_tipo_tramite == filtro.TramiteId);
            }
            if (!string.IsNullOrWhiteSpace(filtro.Radicado))
            {
                query = query.Where(x => x.antecedente.numero_sgdea.Equals(filtro.Radicado));
            }

            var listado = query.OrderBy(x => x.antecedente.id_estado_antecedente).Select(m => new ListadoEstupefacientesDTO
            {
                Id = m.antecedente.id_antecedente,
                Nombre = m.usuario.nombres,
                Apellido = m.usuario.apellidos,
                Radicado = m.antecedente.numero_sgdea,
                Tramite = m.tramite.descripcion_tipo_tramite,
                Capitania = m.capitaniaFirma.SIGLA_CAPITANIA + "-" + m.capitaniaFirma.DESCRIPCION,
                Documento = m.usuario.identificacion,
                Estado = m.estado.descripcion_estado_antecedente,
                FechaAprobacion = m.antecedente.fecha_aprobacion,
                FechaSolicitudSedeCentral = m.antecedente.fecha_solicitud_sede_central,
                FechaVigencia = m.antecedente.fecha_vigencia,
            });
            return listado;
        }


        public async Task<IEnumerable<string>> GetRadicadosDeEstupefacientes(string identificacion)
        {
            return await (from antecedente in _context.GENTEMAR_ANTECEDENTES
                          join usuario in _context.GENTEMAR_ANTECEDENTES_DATOSBASICOS on antecedente.id_gentemar_antecedente equals usuario.id_gentemar_antecedente
                          where usuario.identificacion.Equals(identificacion)
                          select antecedente.numero_sgdea).ToListAsync();
        }
        public async Task<IEnumerable<string>> GetRadicadosTitulosByDocumento(string identificacion)
        {
            var list = await GetRadicadosDeEstupefacientes(identificacion);

            return await (from titulo in _context.GENTEMAR_TITULOS
                          join usuario in _context.GENTEMAR_DATOSBASICOS on titulo.id_gentemar equals usuario.id_gentemar
                          where usuario.documento_identificacion.Equals(identificacion) && !list.Contains(titulo.radicado)
                          select titulo.radicado).ToListAsync();
        }

        public async Task<IEnumerable<string>> GetRadicadosLicenciasByDocumento(string identificacion)
        {
            var list = await GetRadicadosDeEstupefacientes(identificacion);
            return await (from titulo in _context.GENTEMAR_LICENCIAS
                          join usuario in _context.GENTEMAR_DATOSBASICOS on titulo.id_gentemar equals usuario.id_gentemar
                          where usuario.documento_identificacion.Equals(identificacion) && !list.Contains(titulo.radicado)
                          select titulo.radicado).ToListAsync();
        }

        public async Task<UsuarioGenteMarDTO> GetDatosGenteMarEstupefaciente(string identificacionConPuntos)
        {
            return await (from GenteMar in _context.GENTEMAR_ANTECEDENTES_DATOSBASICOS
                          join tipoDocumento in _context.APLICACIONES_TIPO_DOCUMENTO on GenteMar.id_tipo_documento
                          equals tipoDocumento.ID_TIPO_DOCUMENTO
                          where GenteMar.identificacion.Equals(identificacionConPuntos)
                          select new
                          {
                              GenteMar,
                              tipoDocumento,
                          }).Select(x => new UsuarioGenteMarDTO()
                          {
                              Nombres = x.GenteMar.nombres,
                              Apellidos = x.GenteMar.apellidos,
                              DocumentoIdentificacion = x.GenteMar.identificacion,
                              Id = x.GenteMar.id_gentemar_antecedente,
                              IdTipoDocumento = x.tipoDocumento.ID_TIPO_DOCUMENTO,
                              FechaNacimiento = x.GenteMar.fecha_nacimiento,
                          }).FirstOrDefaultAsync();
        }

        public async Task CreateWithGenteMar(GENTEMAR_ANTECEDENTES entidad, GENTEMAR_ANTECEDENTES_DATOSBASICOS datosBasicosAntecedentes)
        {
            using (_context)
            {
                using (var trassaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.GENTEMAR_ANTECEDENTES_DATOSBASICOS.Add(datosBasicosAntecedentes);
                        await SaveAllAsync();
                        entidad.id_gentemar_antecedente = datosBasicosAntecedentes.id_gentemar_antecedente;
                        _context.GENTEMAR_ANTECEDENTES.Add(entidad);
                        await SaveAllAsync();
                        trassaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        trassaction.Rollback();
                        ObtenerException(ex, entidad);
                    }
                }
            }
        }

        public async Task<long> GetIdGenteMarAntecedente(string identificacion)
        {
            return await _context.GENTEMAR_ANTECEDENTES_DATOSBASICOS.Where(x => x.identificacion.Equals(identificacion))
                .Select(x => x.id_gentemar_antecedente).FirstOrDefaultAsync();
        }

        public async Task<DetalleEstupefacienteDTO> GetDetallePersonaEstupefaciente(long id)
        {
            var query = await (from antecedenteGenteMar in _context.GENTEMAR_ANTECEDENTES_DATOSBASICOS
                               join antecedente in _context.GENTEMAR_ANTECEDENTES on antecedenteGenteMar.id_gentemar_antecedente equals antecedente.id_gentemar_antecedente
                               join estado in _context.GENTEMAR_ESTADO_ANTECEDENTES on antecedente.id_estado_antecedente equals estado.id_estado_antecedente
                               join tramite in _context.GENTEMAR_TIPO_TRAMITE on antecedente.id_tipo_tramite equals tramite.id_tipo_tramite
                               join capitania in _context.APLICACIONES_CAPITANIAS on antecedente.id_capitania equals capitania.ID_CAPITANIA
                               join tipoDocumento in _context.APLICACIONES_TIPO_DOCUMENTO on antecedenteGenteMar.id_tipo_documento equals tipoDocumento.ID_TIPO_DOCUMENTO
                               where antecedente.id_antecedente == id
                               select new
                               {
                                   antecedenteGenteMar,
                                   tipoDocumento,
                                   antecedente,
                                   estado,
                                   tramite,
                                   capitania
                               }).Select(x => new DetalleEstupefacienteDTO()
                               {
                                   AntecedenteDatosBasicosId = x.antecedenteGenteMar.id_gentemar_antecedente,
                                   EstupefacienteId = x.antecedente.id_antecedente,
                                   CapitaniaId = x.capitania.ID_CAPITANIA,
                                   EstadoId = x.estado.id_estado_antecedente,
                                   TramiteId = x.tramite.id_tipo_tramite,
                                   DatosBasicos = new EstupefacienteDatosBasicosDTO
                                   {
                                       Nombres = x.antecedenteGenteMar.nombres,
                                       Apellidos = x.antecedenteGenteMar.apellidos,
                                       Identificacion = x.antecedenteGenteMar.identificacion,
                                       FechaNacimiento = x.antecedenteGenteMar.fecha_nacimiento,
                                   },
                                   Estado = x.estado.descripcion_estado_antecedente,
                                   FechaAprobacion = x.antecedente.fecha_aprobacion,
                                   FechaRadicadoSgdea = x.antecedente.fecha_sgdea,
                                   FechaSolicitudSedeCentral = x.antecedente.fecha_solicitud_sede_central,
                                   Radicado = x.antecedente.numero_sgdea,
                                   FechaVigencia = x.antecedente.fecha_vigencia,
                                   Tramite = x.tramite.descripcion_tipo_tramite,
                                   Capitania = x.capitania.SIGLA_CAPITANIA + " - " + x.capitania.DESCRIPCION
                               }).FirstOrDefaultAsync();

            return query;
        }
    }
}

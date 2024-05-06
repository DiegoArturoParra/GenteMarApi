using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.UIEntities.QueryFilters.Reports;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class CargoLicenciaRepository : GenericRepository<GENTEMAR_CARGO_LICENCIA>
    {
        public async Task<IEnumerable<CargoInfoLicenciaDTO>> GetCargosLicenciaActivosPorCapitaniaCategoria(int idCategoria)
        {
            var query = await (from cargolicencia in _context.GENTEMAR_CARGO_LICENCIA
                               join cargoLicenciaCategoria in _context.GENTEMAR_CARGO_LICENCIA_CATEGORIA
                               on cargolicencia.id_cargo_licencia equals cargoLicenciaCategoria.id_cargo_licencia
                               where cargoLicenciaCategoria.id_categoria == idCategoria && cargolicencia.activo == Constantes.ACTIVO
                               select new CargoInfoLicenciaDTO
                               {
                                   IdCargoLicencia = cargolicencia.id_cargo_licencia,
                                   Activo = cargolicencia.activo,
                                   CodigoLicencia = cargolicencia.codigo_licencia,
                                   CargoLicencia = cargolicencia.cargo_licencia,
                                   Nave = cargolicencia.nave
                               }).OrderBy(x => x.CargoLicencia).AsNoTracking().ToListAsync();
            return query;
        }


        /// <summary>
        /// Metodo para actualizar los cargo licencia con limitaciones.
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public async Task ModificarCargoLimitacion(GENTEMAR_CARGO_LICENCIA entidad)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                //await Create(entidad);
                try
                {
                    _context.GENTEMAR_CARGO_LICENCIA.Attach(entidad);
                    var entry = _context.Entry(entidad);
                    entry.State = EntityState.Modified;
                    await SaveAllAsync();
                    // elimina los resgistros de las limitaciones
                    _context.GENTEMAR_CARGO_LIMITACION.RemoveRange(
                        _context.GENTEMAR_CARGO_LIMITACION.Where(x => x.id_cargo_licencia == entidad.id_cargo_licencia)
                    );
                    // elimina los resgistros de las limitantes
                    _context.GENTEMAR_CARGO_LICENCIA_LIMITANTE.RemoveRange(
                        _context.GENTEMAR_CARGO_LICENCIA_LIMITANTE.Where(x => x.id_cargo_licencia == entidad.id_cargo_licencia)
                    );
                    // elimina los registros de las categotias
                    _context.GENTEMAR_CARGO_LICENCIA_CATEGORIA.RemoveRange(_context.GENTEMAR_CARGO_LICENCIA_CATEGORIA
                        .Where(x => x.id_cargo_licencia == entidad.id_cargo_licencia));
                    await CrateInCascadeLimitacion(entidad.IdLimitacion, entidad.id_cargo_licencia);
                    await CrateInCascadeCategoria(entidad.IdCategoria, entidad.id_cargo_licencia);
                    await CrateInCascadeLimitante(entidad.IdLimitante, entidad.id_cargo_licencia);

                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ObtenerException(ex, entidad);
                }
            }
        }
        /// <summary>
        /// Metodo para crear los cargo licencia con limitaciones.
        /// </summary>
        /// <param name="entidad"></param>
        /// <param name="entidadDTO"></param>
        /// <returns></returns>
        public async Task CrearCargoLimitacion(GENTEMAR_CARGO_LICENCIA entidad)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                //await Create(entidad);
                try
                {

                    entidad.activo = true;
                    _context.GENTEMAR_CARGO_LICENCIA.Add(entidad);
                    await SaveAllAsync();
                    await CrateInCascadeLimitacion(entidad.IdLimitacion, entidad.id_cargo_licencia);
                    await CrateInCascadeCategoria(entidad.IdCategoria, entidad.id_cargo_licencia);
                    await CrateInCascadeLimitante(entidad.IdLimitante, entidad.id_cargo_licencia);
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ObtenerException(ex, entidad);
                }
            }
        }
        /// <summary>
        /// Metodo para listar los cargos licencia por id.
        /// </summary>
        /// <param name="entidad"></param>
        /// <param name="IdLimitacion"></param>
        /// <returns></returns>
        public async Task<CargoInfoLicenciaDTO> GetCargoLicenciaIdAsync(long id)
        {
            var temp = (from cargoLicencia in _context.GENTEMAR_CARGO_LICENCIA
                        join actividadSeccion in _context.GENTEMAR_ACTIVIDAD_SECCION_LICENCIA
                        on cargoLicencia.id_actividad_seccion_licencia equals actividadSeccion.id_actividad_seccion_licencia
                        join seccionClase in _context.GENTEMAR_SECCION_CLASE
                        on cargoLicencia.id_seccion_clase equals seccionClase.id_seccion_clase
                        join actividad in _context.GENTEMAR_ACTIVIDAD
                        on actividadSeccion.id_actividad equals actividad.id_actividad
                        where cargoLicencia.id_cargo_licencia == id
                        select new
                        {
                            cargoLicencia,
                            actividadSeccion,
                            seccionClase,
                            actividad

                        });

            var query = await (from data in temp
                               select new CargoInfoLicenciaDTO
                               {
                                   IdCargoLicencia = data.cargoLicencia.id_cargo_licencia,
                                   Activo = data.cargoLicencia.activo,
                                   CargoLicencia = data.cargoLicencia.cargo_licencia,
                                   CodigoLicencia = data.cargoLicencia.codigo_licencia,
                                   Vigencia = data.cargoLicencia.vigencia,
                                   IdActividad = data.actividadSeccion.id_actividad,
                                   IdSeccion = data.actividadSeccion.id_seccion,
                                   IdClase = data.seccionClase.id_clase,
                                   IdActividadSeccion = data.actividadSeccion.id_actividad_seccion_licencia,
                                   IdSeccionClase = data.seccionClase.id_seccion_clase,
                                   IdTipoLicencia = data.actividad.id_tipo_licencia,
                                   Nave = data.cargoLicencia.nave,
                                   IdCategoria = (from categoria in _context.APLICACIONES_CATEGORIA
                                                  join cargoCategoria in _context.GENTEMAR_CARGO_LICENCIA_CATEGORIA
                                                  on categoria.ID_CATEGORIA equals cargoCategoria.id_categoria
                                                  where cargoCategoria.id_cargo_licencia == id
                                                  select new
                                                  {
                                                      categoria

                                                  }).Select(x => x.categoria.ID_CATEGORIA).ToList(),
                                   IdLimitacion = (from limitacion in _context.GENTEMAR_LIMITACION
                                                   join cargoLimitacion in _context.GENTEMAR_CARGO_LIMITACION
                                                   on limitacion.id_limitacion equals cargoLimitacion.id_limitacion
                                                   where cargoLimitacion.id_cargo_licencia == id
                                                   select new
                                                   {
                                                       limitacion
                                                   }).Select(x => x.limitacion.id_limitacion).ToList(),
                                   IdLimitante = (from limtante in _context.GENTEMAR_LIMITANTE
                                                  join cargoLimitante in _context.GENTEMAR_CARGO_LICENCIA_LIMITANTE
                                                  on limtante.id_limitante equals cargoLimitante.id_limitante
                                                  where cargoLimitante.id_cargo_licencia == id
                                                  select new
                                                  {
                                                      limtante
                                                  }).Select(x => x.limtante.id_limitante).ToList()
                               }
                         ).AsNoTracking().FirstOrDefaultAsync();

            return query;
        }

        /// <summary>
        /// metodo para obtener los datos del usuario y de la licencia para la plantilla
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CargoInfoLicenciaDTO> GetCargoLicenciaIdDetalleAsync(long id)
        {
            var temp = (from cargoLicencia in _context.GENTEMAR_CARGO_LICENCIA
                        join actividadSeccion in _context.GENTEMAR_ACTIVIDAD_SECCION_LICENCIA
                        on cargoLicencia.id_actividad_seccion_licencia equals actividadSeccion.id_actividad_seccion_licencia
                        join seccionClase in _context.GENTEMAR_SECCION_CLASE
                        on cargoLicencia.id_seccion_clase equals seccionClase.id_seccion_clase
                        join actividad in _context.GENTEMAR_ACTIVIDAD
                        on actividadSeccion.id_actividad equals actividad.id_actividad
                        where cargoLicencia.id_cargo_licencia == id
                        select new
                        {
                            cargoLicencia,
                            actividadSeccion,
                            seccionClase,
                            actividad

                        });

            var query = await (from data in temp
                               select new CargoLicenciaDetalleDTO
                               {
                                   IdCargoLicencia = data.cargoLicencia.id_cargo_licencia,
                                   Activo = data.cargoLicencia.activo,
                                   CargoLicencia = data.cargoLicencia.cargo_licencia,
                                   CodigoLicencia = data.cargoLicencia.codigo_licencia,
                                   Vigencia = data.cargoLicencia.vigencia,
                                   IdActividad = data.actividadSeccion.id_actividad,
                                   IdSeccion = data.actividadSeccion.id_seccion,
                                   IdClase = data.seccionClase.id_clase,
                                   IdActividadSeccion = data.actividadSeccion.id_actividad_seccion_licencia,
                                   IdSeccionClase = data.seccionClase.id_seccion_clase,
                                   IdTipoLicencia = data.actividad.id_tipo_licencia,

                                   Categoria = (from categoria in _context.APLICACIONES_CATEGORIA
                                                join cargoCategoria in _context.GENTEMAR_CARGO_LICENCIA_CATEGORIA
                                                on categoria.ID_CATEGORIA equals cargoCategoria.id_categoria
                                                where cargoCategoria.id_cargo_licencia == id
                                                select new CategoriaDTO
                                                {
                                                    IdCategoria = categoria.ID_CATEGORIA,
                                                    Descripcion = categoria.DESCRIPCION,
                                                    SiglaCategoria = categoria.SIGLA_CATEGORIA

                                                }).ToList(),
                                   Limitacion = (from limitacion in _context.GENTEMAR_LIMITACION
                                                 join cargoLimitacion in _context.GENTEMAR_CARGO_LIMITACION
                                                 on limitacion.id_limitacion equals cargoLimitacion.id_limitacion
                                                 where cargoLimitacion.id_cargo_licencia == id
                                                 select new LimitacionDTO
                                                 {
                                                     IdLimitacion = limitacion.id_limitacion,
                                                     Limitaciones = limitacion.limitaciones,
                                                     Activo = limitacion.activo,

                                                 }).ToList(),
                                   Clase = (from clase in _context.GENTEMAR_CLASE_LICENCIAS
                                            where clase.id_clase == data.seccionClase.id_clase
                                            select new ClaseDTO
                                            {
                                                Id = clase.id_clase,
                                                Descripcion = clase.descripcion_clase,
                                            }).FirstOrDefault(),
                                   Seccion = (from seccion in _context.GENTEMAR_SECCION_LICENCIAS
                                              where seccion.id_seccion == data.actividadSeccion.id_seccion
                                              select new SeccionDTO
                                              {
                                                  Id = seccion.id_seccion,
                                                  Descripcion = seccion.actividad_a_bordo,
                                              }).FirstOrDefault(),
                                   Actividad = (from actividad in _context.GENTEMAR_ACTIVIDAD
                                                where actividad.id_actividad == data.actividadSeccion.id_actividad
                                                select new ActividadTipoLicenciaDTO
                                                {
                                                    IdActividad = actividad.id_actividad,
                                                    Actividad = actividad.actividad,
                                                }).FirstOrDefault(),
                                   TipoLicencia = (from tipoLicencia in _context.GENTEMAR_TIPO_LICENCIA
                                                   where tipoLicencia.id_tipo_licencia == data.actividad.id_tipo_licencia
                                                   select new TipoLicenciaDTO
                                                   {
                                                       IdTipoLicencia = tipoLicencia.id_tipo_licencia,
                                                       TipoLicencia = tipoLicencia.tipo_licencia,
                                                   }).FirstOrDefault(),
                                   Limitante = (from limitante in _context.GENTEMAR_LIMITANTE
                                                join cargoLimitante in _context.GENTEMAR_CARGO_LICENCIA_LIMITANTE
                                                on limitante.id_limitante equals cargoLimitante.id_limitante
                                                where cargoLimitante.id_cargo_licencia == id
                                                select new LimitanteDTO
                                                {
                                                    IdLimitante = limitante.id_limitante,
                                                    Descripcion = limitante.descripcion,
                                                    Activo = limitante.activo,

                                                }).ToList(),
                                   Nave = data.cargoLicencia.nave,
                               }
                         ).AsNoTracking().FirstOrDefaultAsync();

            return query;
        }

        /// <summary>
        /// Crea las limitaciones del cargo licencia  
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task CrateInCascadeLimitacion(IList<int> data, int id)
        {
            foreach (int item in data)
            {
                var limitacion = new GENTEMAR_CARGO_LIMITACION
                {
                    id_cargo_licencia = id,
                    id_limitacion = item
                };

                _context.GENTEMAR_CARGO_LIMITACION.Add(limitacion);

            }
            await SaveAllAsync();
        }
        /// <summary>
        /// crea las limitantes 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task CrateInCascadeLimitante(IList<int> data, int id)
        {
            foreach (int item in data)
            {
                var limitacion = new GENTEMAR_CARGO_LICENCIA_LIMITANTE
                {
                    id_cargo_licencia = id,
                    id_limitante = item
                };

                _context.GENTEMAR_CARGO_LICENCIA_LIMITANTE.Add(limitacion);

            }
            await SaveAllAsync();
        }
        /// <summary>
        /// Crea las Categorias del cargo licencia 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task CrateInCascadeCategoria(IList<int> data, int id)
        {
            foreach (int item in data)
            {
                var categoria = new GENTEMAR_CARGO_LICENCIA_CATEGORIA
                {
                    id_cargo_licencia = id,
                    id_categoria = item,
                    activo = Constantes.ACTIVO
                };

                _context.GENTEMAR_CARGO_LICENCIA_CATEGORIA.Add(categoria);

            }
            await SaveAllAsync();
        }

        /// <summary>
        /// metodo para obtener los datos del usuario y de la licencia para la plantilla
        /// por id de la licencia.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PlantillaLicenciaDTO> GetPlantillaLicencias(long id)
        {
            var query = await (from licencia in _context.GENTEMAR_LICENCIAS
                               join datosBasicos in _context.GENTEMAR_DATOSBASICOS on licencia.id_gentemar equals datosBasicos.id_gentemar
                               join ciuExpDoc in _context.TABLA_NAV_BAND on datosBasicos.cod_pais equals ciuExpDoc.cod_pais
                               join cargoLicencia in _context.GENTEMAR_CARGO_LICENCIA on licencia.id_cargo_licencia equals cargoLicencia.id_cargo_licencia
                               join actSecLicen in _context.GENTEMAR_ACTIVIDAD_SECCION_LICENCIA on cargoLicencia.id_actividad_seccion_licencia equals actSecLicen.id_actividad_seccion_licencia
                               join actividad in _context.GENTEMAR_ACTIVIDAD on actSecLicen.id_actividad equals actividad.id_actividad
                               join tipoLicencia in _context.GENTEMAR_TIPO_LICENCIA on actividad.id_tipo_licencia equals tipoLicencia.id_tipo_licencia
                               where licencia.id_licencia == id
                               select new PlantillaLicenciaDTO
                               {
                                   TipoLicencia = tipoLicencia,
                                   Foto = (from archivo in _context.GENTEMAR_REPOSITORIO_ARCHIVOS
                                           where archivo.IdModulo == datosBasicos.id_gentemar.ToString()
                                           select new
                                           {
                                               archivo
                                           }).OrderByDescending(x => x.archivo.FechaCreacion).Select(x => x.archivo.RutaArchivo).FirstOrDefault(),
                                   NombreCompleto = datosBasicos.nombres + " " + datosBasicos.apellidos,
                                   Documento = datosBasicos.documento_identificacion,
                                   FechaNacimiento = datosBasicos.fecha_nacimiento,
                                   CiudadExpedicion = datosBasicos.id_municipio_expedicion.HasValue
                                                        ? (from munExpDoc in _context.APLICACIONES_MUNICIPIO
                                                           where munExpDoc.ID_MUNICIPIO == datosBasicos.id_municipio_expedicion
                                                           select munExpDoc.NOMBRE_MUNICIPIO + " " + ciuExpDoc.des_pais).FirstOrDefault() : ciuExpDoc.des_pais,
                                   FechaExpedicion = licencia.fecha_expedicion,
                                   FechaVencimiento = licencia.fecha_vencimiento,
                                   NumeroLicencia = licencia.id_licencia,
                                   NombreLicencia = cargoLicencia.cargo_licencia,
                                   Radicado = licencia.radicado,
                                   Limitantes = (from limitantes in _context.GENTEMAR_LIMITANTE
                                                 join cargoLimit in _context.GENTEMAR_CARGO_LICENCIA_LIMITANTE on limitantes.id_limitante equals cargoLimit.id_limitante
                                                 where cargoLimit.id_cargo_licencia == cargoLicencia.id_cargo_licencia
                                                 select new
                                                 {
                                                     limitantes
                                                 }).Select(x => x.limitantes.descripcion).ToList(),
                                   Limitacion = (from limitacion in _context.GENTEMAR_LIMITACION
                                                 join cargolimta in _context.GENTEMAR_CARGO_LIMITACION on limitacion.id_limitacion equals cargolimta.id_limitacion
                                                 where cargolimta.id_cargo_licencia == cargoLicencia.id_cargo_licencia
                                                 select new
                                                 {
                                                     limitacion
                                                 }).Select(x => x.limitacion.limitaciones).ToList(),
                                   CapitaniaFirmante = (from capitanias in _context.APLICACIONES_CAPITANIAS
                                                        where capitanias.ID_CAPITANIA == licencia.id_capitania_firmante
                                                        select new
                                                        {
                                                            capitanias
                                                        }).Select(x => x.capitanias.DESCRIPCION).FirstOrDefault(),
                                   ActividadLicencia = actividad,
                                   Naves = (from licNav in _context.GENTEMAR_LICENCIA_NAVES
                                            join naves in _context.NAVES_BASE on licNav.identi equals naves.identi
                                            where licNav.id_licencia == licencia.id_licencia
                                            select new NavesImpDocDTO
                                            {
                                                NombreNave = naves.nom_naves,
                                                MatriculaNave = naves.identi
                                            }).ToList()
                               }).AsNoTracking().FirstOrDefaultAsync();

            return query;
        }

        public async Task<IEnumerable<CargoLicenciaDTO>> GetCargosLicenciaActivosPorFiltroParaReporte(CargoLicenciaReportFilter cargoLicenciaFilter)
        {
            var query = (from cargolicencia in _context.GENTEMAR_CARGO_LICENCIA
                         join actividadSeccion in _context.GENTEMAR_ACTIVIDAD_SECCION_LICENCIA
                         on cargolicencia.id_actividad_seccion_licencia
                         equals actividadSeccion.id_actividad_seccion_licencia
                         join seccionClase in _context.GENTEMAR_SECCION_CLASE
                         on cargolicencia.id_seccion_clase equals seccionClase.id_seccion_clase
                         join actividad in _context.GENTEMAR_ACTIVIDAD on actividadSeccion.id_actividad equals actividad.id_actividad
                         join seccion in _context.GENTEMAR_SECCION_LICENCIAS on actividadSeccion.id_seccion equals seccion.id_seccion
                         where cargolicencia.activo == Constantes.ACTIVO
                         select new
                         {
                             cargolicencia,
                             seccionClase,
                             actividadSeccion,
                             actividad,
                             seccion
                         });

            if (cargoLicenciaFilter.TiposDeLicenciaId.Any())
            {
                query = query.Where(x => _context.GENTEMAR_ACTIVIDAD.Where(s => cargoLicenciaFilter.TiposDeLicenciaId.Contains(s.id_tipo_licencia))
                .Select(y => y.id_tipo_licencia).Contains(x.actividad.id_tipo_licencia));
            }

            if (cargoLicenciaFilter.ActividadesId.Any())
            {
                query = query.Where(x => cargoLicenciaFilter.ActividadesId.Contains(x.actividadSeccion.id_actividad));
            }

            if (cargoLicenciaFilter.ClasesLicenciaId.Any())
            {
                query = query.Where(x => cargoLicenciaFilter.ClasesLicenciaId.Contains(x.seccionClase.id_clase));
            }

            if (cargoLicenciaFilter.SeccionesLicenciaId.Any())
            {
                query = query.Where(x => cargoLicenciaFilter.SeccionesLicenciaId.Contains(x.seccion.id_seccion));
            }

            return await query.Select(m => new CargoLicenciaDTO
            {
                Id = m.cargolicencia.id_cargo_licencia,
                Descripcion = m.cargolicencia.cargo_licencia,
                IsActive = m.cargolicencia.activo,
            }).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<CargoInfoLicenciaDTO>> GetCargosLicenciaAsync(CargoLicenciaFilter filtro)
        {
            var data = GetAllAsQueryable();
            if (!string.IsNullOrWhiteSpace(filtro.CargoLicencia))
            {
                data = data.Where(x => x.cargo_licencia.Trim().ToLower().Contains(filtro.CargoLicencia.Trim().ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(filtro.CodigoLicencia))
            {
                data = data.Where(x => x.codigo_licencia.Trim().ToLower().Contains(filtro.CodigoLicencia.Trim().ToLower()));
            }

            if (filtro.Activo.HasValue)
            {
                data = data.Where(x => x.activo == filtro.Activo.Value);
            }
            var response = await data.Select(e => new CargoInfoLicenciaDTO
            {
                FechaExpedicion = e.FechaCreacion,
                IdCargoLicencia = e.id_cargo_licencia,
                Activo = e.activo,
                CodigoLicencia = e.codigo_licencia,
                CargoLicencia = e.cargo_licencia,
                Vigencia = e.vigencia,
            }).OrderByDescending(y => y.FechaExpedicion).AsNoTracking().ToListAsync();
            return response;
        }
    }
}

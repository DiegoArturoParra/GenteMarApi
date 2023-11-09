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
    public class ReglaCargoRepository : GenericRepository<GENTEMAR_REGLAS_CARGO>
    {
        public async Task<bool> ExisteCargoTituloInDetalleRegla(int cargoId)
        {
            return await _context.GENTEMAR_REGLAS_CARGO.AnyAsync(x => x.id_cargo_titulo == cargoId);
        }


        public async Task<CargoReglaDTO> GetDetalleById(int reglaCargoId)
        {
            var query = from cargoRegla in _context.GENTEMAR_REGLAS_CARGO
                        join regla in _context.GENTEMAR_REGLAS on cargoRegla.id_regla equals regla.id_regla
                        join cargoTitulo in _context.GENTEMAR_CARGO_TITULO on cargoRegla.id_cargo_titulo equals cargoTitulo.id_cargo_titulo
                        join nivel in _context.GENTEMAR_NIVEL on cargoRegla.id_nivel equals nivel.id_nivel
                        join capacidad in _context.GENTEMAR_CAPACIDAD on cargoRegla.id_capacidad equals capacidad.id_capacidad
                        join seccionTitulo in _context.GENTEMAR_SECCION_TITULOS on cargoTitulo.id_seccion equals seccionTitulo.id_seccion
                        where cargoRegla.id_cargo_regla == reglaCargoId
                        select new
                        {
                            cargoRegla,
                            regla,
                            cargoTitulo,
                            nivel,
                            seccionTitulo,
                            capacidad
                        };

            var detalle = await query.Select(m => new CargoReglaDTO
            {
                CapacidadId = m.capacidad.id_capacidad,
                CargoReglaId = m.cargoRegla.id_cargo_regla,
                CargoId = m.cargoTitulo.id_cargo_titulo,
                NivelId = m.nivel.id_nivel,
                ReglaId = m.regla.id_regla,
                SeccionId = m.seccionTitulo.id_seccion,
                HabilitacionesId = (from cargoHabilitacion in _context.GENTEMAR_REGLA_CARGO_HABILITACION
                                    join habilitacion in _context.GENTEMAR_HABILITACION on cargoHabilitacion.id_habilitacion
                                    equals habilitacion.id_habilitacion
                                    where cargoHabilitacion.id_cargo_regla == m.cargoRegla.id_cargo_regla
                                    select habilitacion.id_habilitacion).ToList(),
            }).FirstOrDefaultAsync();
            return detalle;
        }

        public async Task<int> GetIdReglaCargo(IdsTablasForaneasDTO idsTablas)
        {
            return await _context.GENTEMAR_REGLAS_CARGO.Where(x => x.id_regla == idsTablas.ReglaId && x.id_cargo_titulo == idsTablas.CargoId
            && x.id_capacidad == idsTablas.CapacidadId && x.id_nivel == idsTablas.NivelId)
                .Select(x => x.id_cargo_regla).FirstOrDefaultAsync();
        }

        public async Task<NivelDTO> GetIdNivelForReglaCargo(IdsTablasForaneasDTO idsTablas)
        {
            return await _context.GENTEMAR_REGLAS_CARGO.Where(x => x.id_regla == idsTablas.ReglaId && x.id_cargo_titulo == idsTablas.CargoId
            && x.id_capacidad == idsTablas.CapacidadId).Include(x => x.GENTEMAR_NIVEL)
                .Select(x => new NivelDTO { Id = x.GENTEMAR_NIVEL.id_nivel, Descripcion = x.GENTEMAR_NIVEL.nivel }).SingleOrDefaultAsync();
        }

        public async Task CrearRelacionReglaCargo(GENTEMAR_REGLAS_CARGO entidad)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.GENTEMAR_REGLAS_CARGO.Add(entidad);
                    await SaveAllAsync();
                    if (entidad.Habilitaciones.Count > 0)
                    {
                        foreach (var item in entidad.Habilitaciones)
                        {
                            var tablaIntermedia = new GENTEMAR_REGLA_CARGO_HABILITACION()
                            {
                                id_habilitacion = item,
                                id_cargo_regla = entidad.id_cargo_regla
                            };
                            _context.GENTEMAR_REGLA_CARGO_HABILITACION.Add(tablaIntermedia);
                        }
                    }
                    await SaveAllAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ObtenerException(ex, entidad);
                }
            }

        }

        public async Task<IEnumerable<ListadoDetalleCargoReglaDTO>> GetListado(DetalleReglaFilter filtro)
        {
            var query = from cargoRegla in _context.GENTEMAR_REGLAS_CARGO
                        join reglas in _context.GENTEMAR_REGLAS on cargoRegla.id_regla equals reglas.id_regla
                        join cargoTitulo in _context.GENTEMAR_CARGO_TITULO on cargoRegla.id_cargo_titulo equals cargoTitulo.id_cargo_titulo
                        join nivel in _context.GENTEMAR_NIVEL on cargoRegla.id_nivel equals nivel.id_nivel
                        join capacidad in _context.GENTEMAR_CAPACIDAD on cargoRegla.id_capacidad equals capacidad.id_capacidad
                        join seccionTitulo in _context.GENTEMAR_SECCION_TITULOS on cargoTitulo.id_seccion equals seccionTitulo.id_seccion
                        select new
                        {
                            cargoRegla,
                            reglas,
                            cargoTitulo,
                            nivel,
                            seccionTitulo,
                            capacidad
                        };

            if (filtro != null)
            {
                if (filtro.SeccionId.HasValue)
                {
                    query = query.Where(x => x.seccionTitulo.id_seccion == filtro.SeccionId.Value);
                }
                if (filtro.CargoTituloId.HasValue)
                {
                    query = query.Where(x => x.cargoRegla.id_cargo_titulo == filtro.CargoTituloId.Value);
                }
                if (filtro.ReglaId.HasValue)
                {
                    query = query.Where(x => x.cargoRegla.id_regla == filtro.ReglaId.Value);
                }
                if (filtro.NivelId.HasValue)
                {
                    query = query.Where(x => x.cargoRegla.id_nivel == filtro.NivelId.Value);
                }
            }

            var listado = query.GroupBy(x => new
            {
                x.cargoRegla.id_regla,
                x.cargoRegla.id_cargo_titulo,
                x.cargoRegla.id_capacidad,
                x.cargoRegla.id_nivel
            }).Select(m => new ListadoDetalleCargoReglaDTO
            {
                CargoReglaId = m.Select(x => x.cargoRegla.id_cargo_regla).FirstOrDefault(),
                Seccion = m.Select(x => x.seccionTitulo.actividad_a_bordo).FirstOrDefault(),
                CargoTitulo = m.Select(x => x.cargoTitulo.cargo).FirstOrDefault(),
                Nivel = m.Select(x => x.nivel.nivel).FirstOrDefault(),
                Regla = m.Select(x => x.reglas.nombre_regla).FirstOrDefault(),
                Capacidad = m.Select(x => x.capacidad.capacidad).FirstOrDefault(),
                Funciones = (from reglaFuncion in _context.GENTEMAR_REGLA_FUNCION
                             join funciones in _context.GENTEMAR_FUNCIONES on reglaFuncion.id_funcion equals funciones.id_funcion
                             where reglaFuncion.id_regla == m.Key.id_regla
                             select funciones.funcion).ToList(),
                Habilitaciones = (from cargoHabilitacion in _context.GENTEMAR_REGLA_CARGO_HABILITACION
                                  join habilitacion in _context.GENTEMAR_HABILITACION on cargoHabilitacion.id_habilitacion
                                  equals habilitacion.id_habilitacion
                                  where cargoHabilitacion.id_cargo_regla == m.Select(x => x.cargoRegla.id_cargo_regla).FirstOrDefault()
                                  select habilitacion.habilitacion).ToList(),
            });
            return await listado.ToListAsync();
        }

        public async Task ActualizarRelacionReglaCargo(GENTEMAR_REGLAS_CARGO data)
        {
            try
            {
                _context.GENTEMAR_REGLA_CARGO_HABILITACION.RemoveRange(_context.GENTEMAR_REGLA_CARGO_HABILITACION
                    .Where(x => x.id_cargo_regla == data.id_cargo_regla));

                if (data.Habilitaciones.Count > 0)
                {
                    foreach (var item in data.Habilitaciones)
                    {
                        var tablaIntermedia = new GENTEMAR_REGLA_CARGO_HABILITACION()
                        {
                            id_habilitacion = item,
                            id_cargo_regla = data.id_cargo_regla
                        };
                        _context.GENTEMAR_REGLA_CARGO_HABILITACION.Add(tablaIntermedia);
                    }
                }
                await Update(data);
            }
            catch (Exception ex)
            {
                ObtenerException(ex, data);
            }
        }
    }
}

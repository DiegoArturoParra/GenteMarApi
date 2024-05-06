using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class GradoRepository : GenericRepository<APLICACIONES_GRADO>
    {
        public async Task<IEnumerable<GradoDTO>> GetGradosActivos()
        {
            return await _context.APLICACIONES_GRADO.Where(x => x.activo == Constantes.ACTIVO).Select(x => new GradoDTO
            {
                Id = x.id_grado,
                Descripcion = x.grado,
                IsActive = x.activo
            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Lista de grados por formacion id
        /// </summary>
        /// <returns>Lista de grados por formacion id</returns>
        public async Task<IList<GradoInfoDTO>> GetGradosPorFormacionId(int id, bool status)
        {

            var resultado = from a in _context.APLICACIONES_GRADO
                            join gfg in _context.GENTEMAR_FORMACION_GRADO on a.id_grado equals gfg.id_grado
                            where gfg.id_formacion == id
                            select new GradoInfoDTO
                            {
                                grado = a.grado,
                                id_grado = a.id_grado,
                                id_rango = a.id_rango,
                                sigla = a.sigla,
                                id_formacion_grado = gfg.id_formacion_grado,
                                activo = a.activo
                            };

            if (status)
                resultado = resultado.Where(x => x.activo == status);

            return await resultado.OrderBy(x => x.grado).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// Lista de grados con formacion
        /// </summary>
        /// <returns>Lista de grados con formacion</returns>>
        public async Task<IList<GradoInfoDTO>> GetGradosConFormacion(bool isActivos = false)
        {
            var resultado = from grado in _context.APLICACIONES_GRADO
                            join rango in _context.APLICACIONES_RANGO on grado.id_rango equals rango.id_rango
                            select new GradoInfoDTO
                            {
                                id_grado = grado.id_grado,
                                grado = grado.grado,
                                id_rango = grado.id_rango,
                                sigla = grado.sigla,
                                activo = grado.activo,
                                nombreRango = rango.rango,
                                formacion = (from formacionGrado in _context.GENTEMAR_FORMACION_GRADO
                                             join formacion in _context.GENTEMAR_FORMACION on formacionGrado.id_formacion equals formacion.id_formacion
                                             where formacionGrado.id_grado == grado.id_grado
                                             select new FormacionDTO
                                             {
                                                 id_formacion = formacion.id_formacion,
                                                 formacion = formacion.formacion,
                                                 id_formacion_grado = formacionGrado.id_formacion_grado,
                                                 activo = formacion.activo
                                             }).FirstOrDefault()
                            };

            if (isActivos)
            {
                resultado = resultado.Where(x => x.activo == Constantes.ACTIVO);
            }
            return await resultado.OrderBy(x => x.grado).AsNoTracking().ToListAsync();
        }


        /// <summary>
        /// Metodo para crear los grados asociandolo a su formacion
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task CrearGrados(GradoInfoDTO data)
        {

            using (var transaction = _context.Database.BeginTransaction())
            {
                var grado = new APLICACIONES_GRADO();
                try
                {
                    grado.grado = data.grado;
                    grado.sigla = data.sigla;
                    grado.id_rango = data.id_rango.Value;
                    grado.activo = data.activo;
                    _context.APLICACIONES_GRADO.Add(grado);
                    await SaveAllAsync();
                    var formacionGrado = new GENTEMAR_FORMACION_GRADO
                    {
                        id_formacion = data.formacion.id_formacion.Value,
                        id_grado = grado.id_grado
                    };
                    _context.GENTEMAR_FORMACION_GRADO.Add(formacionGrado);
                    await SaveAllAsync();
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ObtenerException(ex, grado);
                }
            }
        }


        public async Task ActualizarGrados(GradoInfoDTO data)
        {
            var grado = new APLICACIONES_GRADO();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    grado.id_grado = (int)data.id_grado; //revisar excepcion que no salta cuando se genra un error  
                    grado.grado = data.grado;
                    grado.sigla = data.sigla;
                    grado.id_rango = data.id_rango.Value;
                    grado.activo = data.activo;

                    _context.APLICACIONES_GRADO.Attach(grado);
                    var entry = _context.Entry(grado);
                    entry.State = EntityState.Modified;
                    await SaveAllAsync();
                    var formacionGrado = _context.GENTEMAR_FORMACION_GRADO.Where(x => x.id_formacion_grado == data.formacion.id_formacion_grado).FirstOrDefault();
                    if (formacionGrado != null)
                    {
                        formacionGrado.id_formacion = data.formacion.id_formacion.Value;
                        formacionGrado.id_grado = grado.id_grado;
                        _context.GENTEMAR_FORMACION_GRADO.Attach(formacionGrado);
                        var entry2 = _context.Entry(formacionGrado);
                        entry2.State = EntityState.Modified;
                    }
                    else
                    {
                        formacionGrado = new GENTEMAR_FORMACION_GRADO
                        {
                            id_formacion = data.formacion.id_formacion.Value,
                            id_grado = grado.id_grado
                        };
                        _context.GENTEMAR_FORMACION_GRADO.Add(formacionGrado);
                    }
                    await SaveAllAsync();
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ObtenerException(ex, grado);
                }
            }
        }
    }
}

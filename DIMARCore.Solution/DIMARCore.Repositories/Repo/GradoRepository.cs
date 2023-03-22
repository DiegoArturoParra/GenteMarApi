using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repo
{
    public class GradoRepository : GenericRepository<APLICACIONES_GRADO>
    {
        GenteDeMarCoreContext contexto = new GenteDeMarCoreContext();
        /// <summary>
        /// Lista de formacion 
        /// </summary>
        /// <returns>Lista de los tipos de formacion</returns>
        public IList<GradoDTO> GetGradoIdGrado(int id)
        {
            try
            {
                var resultado = (from a in contexto.APLICACIONES_GRADO
                                 join gfg in contexto.GENTEMAR_FORMACION_GRADO on a.id_grado equals gfg.id_grado
                                 where gfg.id_formacion == id
                                 select new GradoDTO
                                 {
                                     grado = a.grado,
                                     id_grado = a.id_grado,
                                     id_rango = a.id_rango,
                                     sigla = a.sigla,
                                     id_formacion_grado = gfg.id_formacion_grado,
                                     activo = a.activo

                                 }
                                 ).OrderBy(p => p.grado).ToList();

                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// Lista de formacion 
        /// </summary>
        /// <returns>Lista de los tipos de formacion</returns>
        public IList<GradoDTO> GetGrado()
        {
            try
            {
                var resultado = (from grado in contexto.APLICACIONES_GRADO
                                 join rango in contexto.APLICACIONES_RANGO on grado.id_rango equals rango.id_rango
                                 select new GradoDTO
                                 {
                                     id_grado = grado.id_grado,
                                     grado = grado.grado,
                                     id_rango = grado.id_rango,
                                     sigla = grado.sigla,
                                     activo = grado.activo,
                                     nombreRango = rango.rango,
                                     formacion = (from formacionGrado in contexto.GENTEMAR_FORMACION_GRADO
                                                  join formacion in contexto.GENTEMAR_FORMACION on formacionGrado.id_formacion equals formacion.id_formacion
                                                  where formacionGrado.id_grado == grado.id_grado
                                                  select new FormacionDTO
                                                  {
                                                      id_formacion = formacion.id_formacion,
                                                      formacion = formacion.formacion,
                                                      id_formacion_grado = formacionGrado.id_formacion_grado,
                                                      activo = formacion.activo

                                                  }).FirstOrDefault()
                                 }).OrderBy(x => x.grado).ToList();

                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Metodo para crear los grados asociandolo a su formacion
        /// </summary>
        /// <param name="entidad"></param>
        /// <param name="fotografia"></param>
        /// <returns></returns>
        public async Task CrearGrados(GradoDTO data)
        {
            using (_context)
            {
                using (var trassaction = _context.Database.BeginTransaction())
                {
                    //await Create(entidad);
                    try
                    {
                        var grado = new APLICACIONES_GRADO();
                        grado.grado = data.grado;
                        grado.sigla = data.sigla;
                        grado.id_rango = data.id_rango;
                        grado.activo = data.activo;
                        _context.APLICACIONES_GRADO.Add(grado);
                        await SaveAllAsync();
                        var formacionGrado = new GENTEMAR_FORMACION_GRADO();
                        formacionGrado.id_formacion = data.formacion.id_formacion;
                        formacionGrado.id_grado = grado.id_grado;
                        _context.GENTEMAR_FORMACION_GRADO.Add(formacionGrado);
                        await SaveAllAsync();
                        trassaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        trassaction.Rollback();
                        GetInnerException<Exception>(ex);

                    }
                }
            }
        }

        /// <summary>
        /// Metodo para crear los grados asociandolo a su formacion
        /// </summary>
        /// <param name="entidad"></param>
        /// <param name="fotografia"></param>
        /// <returns></returns>
        public async Task actualizarGrados(GradoDTO data)
        {
            using (_context)
            {
                using (var trassaction = _context.Database.BeginTransaction())
                {
                    //await Create(entidad);
                    try
                    {
                        var grado = new APLICACIONES_GRADO();
                        grado.id_grado = (int)data.id_grado; //revisar excepcion que no salta cuando se genra un error  
                        grado.grado = data.grado;
                        grado.sigla = data.sigla;
                        grado.id_rango = data.id_rango;
                        grado.activo = data.activo;

                        _context.APLICACIONES_GRADO.Attach(grado);
                        var entry = _context.Entry(grado);
                        entry.State = EntityState.Modified;
                        await SaveAllAsync();
                        var formacionGrado = _context.GENTEMAR_FORMACION_GRADO.Where(x => x.id_formacion_grado == data.formacion.id_formacion_grado).FirstOrDefault();
                        formacionGrado.id_formacion = data.formacion.id_formacion;
                        formacionGrado.id_grado = grado.id_grado;
                        _context.GENTEMAR_FORMACION_GRADO.Attach(formacionGrado);
                        var entry2 = _context.Entry(formacionGrado);
                        entry2.State = EntityState.Modified;
                        await SaveAllAsync();
                        trassaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        trassaction.Rollback();
                        GetInnerException<Exception>(ex);
                        throw ex;

                    }
                }
            }
        }
    }
}

using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DIMARCore.Repositories.Repo
{
    public class FormacionRepository : GenericRepository<GENTEMAR_FORMACION>
    {
        GenteDeMarCoreContext contexto = new GenteDeMarCoreContext();
        /// <summary>
        /// Lista de formacion con los grados asignados 
        /// </summary>
        /// <returns>Lista de los tipos de formacion</returns>
        public IList<FormacionDTO> GetTableFormacion()
        {
            try
            {
                var resultado = (from formacion in contexto.GENTEMAR_FORMACION
                                 select new FormacionDTO
                                 {
                                     formacion = formacion.formacion,
                                     id_formacion = formacion.id_formacion,
                                     activo = formacion.activo,
                                     listaGrado = (from formacionGrado in contexto.GENTEMAR_FORMACION_GRADO
                                                   join grado in contexto.APLICACIONES_GRADO on formacionGrado.id_grado equals grado.id_grado
                                                   where formacionGrado.id_formacion == formacion.id_formacion
                                                   select new GradoDTO
                                                   {
                                                       grado = grado.grado,
                                                       id_grado = grado.id_grado,
                                                       sigla = grado.sigla,
                                                       id_rango = grado.id_rango,
                                                       id_formacion_grado = formacionGrado.id_formacion_grado
                                                   }).ToList()
                                 }).ToList();

                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// lista todas las formaciones sie stado es true lista las formaciones  activas 
        /// si es false trae todas las formaciones  
        /// </summary>
        /// <param name="estado"></param>
        /// <returns></returns>
        public IList<FormacionDTO> GetFormacion(bool estado)
        {
            try
            {
                var resultado = (from formacion in contexto.GENTEMAR_FORMACION
                                 select new FormacionDTO
                                 {
                                     formacion = formacion.formacion,
                                     id_formacion = formacion.id_formacion,
                                     activo = formacion.activo,
                                 });
                if (estado)
                {
                    resultado = resultado.Where(x => x.activo == estado);
                    
                }

                return resultado.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

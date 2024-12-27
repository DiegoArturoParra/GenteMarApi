using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class FormacionRepository : GenericRepository<GENTEMAR_FORMACION>
    {

        /// <summary>
        /// Lista de formacion con los grados asignados 
        /// </summary>
        /// <returns>Lista de los tipos de formacion</returns>
        public async Task<IList<FormacionDTO>> GetTableFormacion(bool isActivo = false)
        {

            var resultado = from formacion in _context.GENTEMAR_FORMACION
                            select new FormacionDTO
                            {
                                formacion = formacion.formacion,
                                id_formacion = formacion.id_formacion,
                                activo = formacion.activo,
                                listaGrado = (from formacionGrado in _context.GENTEMAR_FORMACION_GRADO
                                              join grado in _context.APLICACIONES_GRADO on formacionGrado.id_grado equals grado.id_grado
                                              where formacionGrado.id_formacion == formacion.id_formacion
                                              select new GradoInfoDTO
                                              {
                                                  grado = grado.grado,
                                                  id_grado = grado.id_grado,
                                                  sigla = grado.sigla,
                                                  id_rango = grado.id_rango,
                                                  id_formacion_grado = formacionGrado.id_formacion_grado
                                              }).ToList()
                            };

            if (isActivo)
                resultado = resultado.Where(x => x.activo == isActivo);

            return await resultado.ToListAsync();
        }

        /// <summary>
        /// lista todas las formaciones sie stado es true lista las formaciones  activas 
        /// si es false trae todas las formaciones  
        /// </summary>
        /// <param name="estado"></param>
        /// <returns></returns>
        public async Task<IList<FormacionDTO>> GetFormaciones(bool estado)
        {

            var resultado = (from formacion in _context.GENTEMAR_FORMACION
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
            return await resultado.ToListAsync();
        }
    }
}

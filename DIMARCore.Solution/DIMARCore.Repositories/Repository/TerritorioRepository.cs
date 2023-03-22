using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Linq;


namespace DIMARCore.Repositories.Repository
{
    public class TerritorioRepository : GenericRepository<GENTEMAR_TERRITORIO>
    {
        /// <summary>
        /// Lista de Territorios
        /// </summary>
        /// <returns>Lista de territorios</returns>
        /// <tabla>GENTEMAR_TERRITORIO</tabla>
        public IList<GENTEMAR_TERRITORIO> GetTerritorios()
        {
            var resultado = (from a in _context.GENTEMAR_TERRITORIO
                             select a).OrderBy(p => p.territorio).ToList();
            return resultado;
        }


        /// <summary>
        /// Territorio dado el Id
        /// </summary>
        /// <param name="id">Id del Territorio</param>
        /// <returns>Territorio</returns>
        /// <tabla>GENTEMAR_TERRITORIO</tabla>
        public GENTEMAR_TERRITORIO GetTerritorio(int id)
        {
            var resultado = (from c in _context.GENTEMAR_TERRITORIO
                             where c.id_territorio == id
                             select c
                            ).FirstOrDefault();
            return resultado;
        }
    }

}

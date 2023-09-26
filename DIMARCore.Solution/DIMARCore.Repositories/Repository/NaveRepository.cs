using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class NaveRepository : GenericRepository<NAVES_BASE>
    {

        /// <summary>
        /// Obtiene el listado de naves 
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<NavesDTO>> GetNaves()
        {
            // Obtiene la lista
            var data = await (from n in _context.NAVES_BASE
                              join nm in _context.TABLA_DETALLE_NAVE on n.identi equals nm.identi
                              where n.cod_pais == Constantes.COLOMBIA_CODIGO && n.activa == Constantes.NAVEACTIVA
                              select new NavesDTO
                              {
                                  NomNaves = n.nom_naves,
                                  CedulaNitPropie = n.cedula_nit_propie,
                                  Clase = n.clase,
                                  Eslora = nm.eslora,
                                  Calado = nm.calado,
                                  CodPais = n.cod_pais,
                                  Cod_andino = nm.cod_andino,
                                  Franco_bordo = nm.franco_bordo,
                                  Grupo = n.grupo,
                                  Identi = nm.identi,
                                  Letras = n.letras,
                                  Linea = n.linea,
                                  Manga = nm.manga,
                                  Puntal = nm.puntal,
                                  Tacarreo = nm.tacarreo,
                                  Tpm = nm.tpm,
                                  Trb = nm.trb,
                                  Trn = nm.trn,
                                  NomArmad = n.nom_armad,
                                  TipoccNitPropie = n.tipo_cc_nit_propie
                              }
                        ).OrderBy(p => p.NomNaves).ToListAsync();
            return data;

        }
    }
}

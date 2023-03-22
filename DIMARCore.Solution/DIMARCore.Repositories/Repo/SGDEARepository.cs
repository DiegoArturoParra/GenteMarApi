using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace DIMARCore.Repositories.Repo
{
    public class SGDEARepository : GenericRepository<SGDEA_PREVISTAS>
    {
        private const string TRAMITE_TITULOS = "TITULOS";
        public IEnumerable<RadicadoDTO> GetRadicadosTitulosByCedula(string cedula)
        {

            List<RadicadoDTO> radicados = new List<RadicadoDTO>();
            radicados = (from listado in _context.TABLA_SGDEA_PREVISTAS
                         where listado.numero_identificacion_usuario.Equals(cedula) && listado.tipo_tramite.Contains(TRAMITE_TITULOS)
                         group listado by listado.radicado into grouped
                         where grouped.Count() == 1
                         select new RadicadoDTO
                         {
                             Radicado = grouped.Key.ToString(),
                             Conteo = grouped.Count()
                         }).ToList();


            return radicados;
        }
    }
}

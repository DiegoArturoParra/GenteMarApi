using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Business.Interfaces
{
    public interface ITitulosBO
    {
        Task<Respuesta> ActualizarAsync(GENTEMAR_TITULOS entidad, string pathActual);
        Task<Respuesta> CrearAsync(GENTEMAR_TITULOS entidad, string pathActual);
        IQueryable<ListadoTituloDTO> GetTitulosQueryable();
    }
}

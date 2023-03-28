using DIMARCore.Utilities.Helpers;
using System.Threading.Tasks;

namespace DIMARCore.Business.Interfaces
{
    public interface IGenericCreateUpdate<T, TID> where T : class
    {
        Task<Respuesta> CrearAsync(T entidad);
        Task<Respuesta> ActualizarAsync(T entidad);

    }
}

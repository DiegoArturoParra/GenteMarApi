using DIMARCore.Utilities.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DIMARCore.Business.Interfaces
{
    public interface IGenericCRUD<T, TID> where T : class
    {
        IEnumerable<T> GetAll(bool? activo = true);
        Task<Respuesta> GetByIdAsync(TID Id);
        Task<Respuesta> CrearAsync(T entidad);
        Task<Respuesta> ActualizarAsync(T entidad);
        Task<Respuesta> AnulaOrActivaAsync(TID Id);
        Task ExisteByNombreAsync(string nombre, TID Id);
    }
}

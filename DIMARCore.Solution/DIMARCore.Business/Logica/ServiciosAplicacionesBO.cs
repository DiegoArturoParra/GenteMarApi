using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace DIMARCore.Business.Logica
{
    public class ServiciosAplicacionesBO
    {

        public async Task<IEnumerable<TipoSolicitudDTO>> GetTiposSolicitud()
        {
            List<string> excepto = new List<string>()
            {
                "RENUEVA",
                "EXPIDE"
            };
            using (AplicacionTipoSolicitudRepository _repo = new AplicacionTipoSolicitudRepository())
            {
                return await _repo.GetTiposSolicitud(excepto);
            }
        }

        /// <summary>
        /// Listar los tipos de refrendos
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TipoRefrendoDTO>> GetTiposRefrendoAsync()
        {
            using (AplicacionTipoRefrendoRepository _repo = new AplicacionTipoRefrendoRepository())
            {
                return await _repo.GetTiposRefrendo();
            }

        }

        public async Task<IEnumerable<CapitaniaDTO>> GetCapitaniasAsync()
        {

            using (AplicacionCapitaniaRepository _capitaniaRepository = new AplicacionCapitaniaRepository())
            {
                return await _capitaniaRepository.GetCapitanias();
            }
        }
    }
}

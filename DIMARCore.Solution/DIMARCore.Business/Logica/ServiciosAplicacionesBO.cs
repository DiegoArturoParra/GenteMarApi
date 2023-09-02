using DIMARCore.Repositories.Repository;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Linq;
namespace DIMARCore.Business.Logica
{
    public class ServiciosAplicacionesBO
    {
        public IList<GENTEMAR_ESTADO_TITULO> GetEstadosTramite()
        {
            using (ServiciosAplicacionesRepository<GENTEMAR_ESTADO_TITULO> _repo = new ServiciosAplicacionesRepository<GENTEMAR_ESTADO_TITULO>())
            {
                return _repo.Table.OrderBy(x => x.id_estado_tramite).ToList();
            }
        }

        public IEnumerable<APLICACIONES_TIPO_SOLICITUD> GetTiposSolicitud()
        {
            List<string> excepto = new List<string>()
            {
                "RENUEVA",
                "EXPIDE"
            };

            using (ServiciosAplicacionesRepository<APLICACIONES_TIPO_SOLICITUD> _repo = new ServiciosAplicacionesRepository<APLICACIONES_TIPO_SOLICITUD>())
            {
                return _repo.Table.Where(x => !excepto.Contains(x.DESCRIPCION)).OrderBy(x => x.ID_TIPO_SOLICITUD).ToList();
            }
        }

        /// <summary>
        /// Listar los tipos de refrendos
        /// </summary>
        /// <returns></returns>
        public IEnumerable<APLICACIONES_TIPO_REFRENDO> GetTipoRefrendos()
        {
            using (ServiciosAplicacionesRepository<APLICACIONES_TIPO_REFRENDO> _repo = new ServiciosAplicacionesRepository<APLICACIONES_TIPO_REFRENDO>())
            {
                return _repo.Table.OrderBy(x => x.DESCRIPCION).ToList();
            }

        }

        public IEnumerable<APLICACIONES_CAPITANIAS> GetCapitanias()
        {

            using (ServiciosAplicacionesRepository<APLICACIONES_CAPITANIAS> _repo = new ServiciosAplicacionesRepository<APLICACIONES_CAPITANIAS>())
            {
                return _repo.Table.OrderBy(x => x.SIGLA_CAPITANIA).ToList();
            }
        }

        public IEnumerable<APLICACIONES_CAPITANIAS> GetCapitaniasFirmante()
        {
            using (ServiciosAplicacionesRepository<APLICACIONES_CAPITANIAS> _repo = new ServiciosAplicacionesRepository<APLICACIONES_CAPITANIAS>())
            {
                return _repo.Table.Where(x => x.SIGLA_CAPITANIA.Equals("DIMAR")).ToList();
            }
        }
    }
}

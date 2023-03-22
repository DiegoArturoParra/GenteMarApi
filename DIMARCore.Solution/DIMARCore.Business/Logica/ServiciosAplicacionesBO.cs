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
            using (ServiciosAplicacionesRepository<GENTEMAR_ESTADO_TITULO> _repo
            = new ServiciosAplicacionesRepository<GENTEMAR_ESTADO_TITULO>())
            {
                var lista = _repo.GetAll().OrderBy(x => x.id_estado_tramite).ToList();
                return lista;
            }
        }

        public IEnumerable<APLICACIONES_TIPO_SOLICITUD> GetTiposSolicitud()
        {
            List<string> excepto = new List<string>()
            {
                "RENUEVA",
                "EXPIDE"
            };

            using (ServiciosAplicacionesRepository<APLICACIONES_TIPO_SOLICITUD> _repo
            = new ServiciosAplicacionesRepository<APLICACIONES_TIPO_SOLICITUD>())
            {
                return _repo.GetAll().Where(x => !excepto.Contains(x.DESCRIPCION)).OrderBy(x => x.ID_TIPO_SOLICITUD).ToList();

            }
        }

        public IEnumerable<APLICACIONES_CAPITANIAS> GetCapitanias()
        {

            using (ServiciosAplicacionesRepository<APLICACIONES_CAPITANIAS> _repo
          = new ServiciosAplicacionesRepository<APLICACIONES_CAPITANIAS>())
            {
                var listado = _repo.GetAll().OrderBy(x => x.ID_CAPITANIA).ToList();
                return listado;
            }

        }

        public IEnumerable<APLICACIONES_CAPITANIAS> GetCapitaniasFirmante()
        {

            using (ServiciosAplicacionesRepository<APLICACIONES_CAPITANIAS> _repo
          = new ServiciosAplicacionesRepository<APLICACIONES_CAPITANIAS>())
            {
                return _repo.GetCapitaniasFirmante();
            }
        }
    }
}

using DIMARCore.Repositories.Repository;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DIMARCore.Utilities.Middleware;
namespace DIMARCore.Business
{
    public class TipoLicenciaBO
    {

        /// <summary>
        /// Lista de Tipo Licencias
        /// </summary>
        /// <returns>Lista de Tipo Licenciass</returns>
        /// <entidad>GENTEMAR_TIPO_LICENCIA</entidad>
        /// <tabla>GENTEMAR_TIPO_LICENCIA</tabla>
        public IList<GENTEMAR_TIPO_LICENCIA> GetTipoLicencias()
        {
            // Obtiene la lista
            return new TipoLicenciaRepository().GetTipoLicencias();
        }

        /// <summary>
        /// Lista de Tipo Licencias
        /// </summary>
        /// <returns>Lista de Tipo Licenciass</returns>
        /// <entidad>GENTEMAR_TIPO_LICENCIA</entidad>
        /// <tabla>GENTEMAR_TIPO_LICENCIA</tabla>
        public IList<GENTEMAR_TIPO_LICENCIA> GetTipoLicenciasActivo()
        {
            // Obtiene la lista
            return new TipoLicenciaRepository().GetAllWithCondition(x => x.activo == true).ToList();
        }


        /// <summary>
        /// Obtener Tipo Licencia dado su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objeto Tipo Licencias dado su id</returns>
        /// <entidad>GENTEMAR_TIPO_LICENCIA</entidad>
        /// <tabla>GENTEMAR_TIPO_LICENCIA</tabla>
        public GENTEMAR_TIPO_LICENCIA GetTipoLicencia(int id)
        {
            return new TipoLicenciaRepository().GetTipoLicencia(id);
        }

        /// <summary>
        /// Crea un tipo de licancia  
        /// </summary>
        /// <param name="datos">Información del tipo de licencia </param>
        /// <returns>Respuesta resultado</returns>
        public async Task<Respuesta> CrearTipoLicenciaAsync(GENTEMAR_TIPO_LICENCIA datos)
        {
            using (var repo = new TipoLicenciaRepository())
            {
                var validate = await repo.GetWithCondition(x => x.tipo_licencia.Equals(datos.tipo_licencia));
                if (validate != null)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse("El tipo de licencia ya esta creada."));
                datos.activo = true;
                await repo.Create(datos);
                return Responses.SetCreatedResponse(datos);
            }

        }


        /// <summary>
        /// Edita la información de un tipo de licencia  
        /// </summary>
        /// <param name="datos">Datos de un tipo de licencia </param>
        /// <param name="usuario">Usuario </param> //Todo: Se deja para implementar Autenticación
        /// <returns>Respuesta resultado</returns>
        public async Task<Respuesta> EditarTipoLicenciaAsync(GENTEMAR_TIPO_LICENCIA datos)
        {
            using (var repo = new TipoLicenciaRepository())
            {
                // busca el tipo de licencia
                var validate = await repo.GetWithCondition(x => x.id_tipo_licencia == datos.id_tipo_licencia);
                if (validate == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El tipo de licencia no existe."));
                datos.id_tipo_licencia = validate.id_tipo_licencia;
                datos.activo = validate.activo;
                await new TipoLicenciaRepository().Update(datos);
                return Responses.SetUpdatedResponse(datos);
            }
        }

        /// <summary>
        /// cambia el estado del rango 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Respuesta> cambiarTipoLicencia(int id)
        {
            using (var repo = new TipoLicenciaRepository())
            {
                var validate = await repo.GetWithCondition(x => x.id_tipo_licencia == id);
                if (validate == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El tipo de licencia no existe"));
                validate.activo = !validate.activo;
                await repo.Update(validate);
                return Responses.SetUpdatedResponse(validate);
            }
        }

    }
}







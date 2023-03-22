using DIMARCore.Repositories.Repository;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Business
{
    public class TerritorioBO
    {

        /// <summary>
        /// Lista de Territorios
        /// </summary>
        /// <returns>Lista de los Territorios</returns>
        /// <entidad>GENTEMAR_TERRITORIO</entidad>
        /// <tabla>GENTEMAR_TERRITORIO</tabla>
        public IList<GENTEMAR_TERRITORIO> GetTerritorios()
        {
            // Obtiene la lista
            return new TerritorioRepository().GetTerritorios();
        }

        /// <summary>
        /// Lista de Territorios
        /// </summary>
        /// <returns>Lista de los Territorios</returns>
        /// <entidad>GENTEMAR_TERRITORIO</entidad>
        /// <tabla>GENTEMAR_TERRITORIO</tabla>
        public IList<GENTEMAR_TERRITORIO> GetTerritoriosActivo()
        {
            // Obtiene la lista
            return new TerritorioRepository().GetAllWithCondition(x => x.activo == true).ToList();
        }


        /// <summary>
        /// Obtener Territorio dado su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objeto Territorio dado su id</returns>
        /// <entidad>GENTEMAR_TERRITORIO</entidad>
        /// <tabla>GENTEMAR_TERRITORIO</tabla>
        public GENTEMAR_TERRITORIO GetTerritorio(int id)
        {
            return new TerritorioRepository().GetTerritorio(id);
        }


        /// <summary>
        /// Crea un Territorio 
        /// </summary>
        /// <param name="datos">Información del Territorio</param>
        /// <returns>Respuesta resultado</returns>
        /// <exception cref="HttpStatusCodeException"></exception>
        public async Task<Respuesta> CrearTerritorioAsync(GENTEMAR_TERRITORIO datos)
        {
            using (var repo = new TerritorioRepository())
            {
                var validate = await repo.AnyWithCondition(x => x.territorio.Equals(datos.territorio));
                if (validate)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse("El Territorio ya esta creado"));
                datos.activo = true;
                await repo.Create(datos);
                return Responses.SetCreatedResponse(datos);
            }
        }


        /// <summary>
        /// Edita la información de un Territorio 
        /// </summary>
        /// <param name="datos">Datos de un Tuerritorio</param>
        /// <param name="usuario">Usuario </param> //Todo: Se deja para implementar Autenticación
        /// <returns>Respuesta resultado</returns>
        /// <exception cref="HttpStatusCodeException"></exception>
        public async Task<Respuesta> EditarTerritorioAsync(GENTEMAR_TERRITORIO datos)
        {
            using (var repo = new TerritorioRepository())
            {
                var validate = await repo.GetWithCondition(x => x.id_territorio == datos.id_territorio);
                if (validate == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El territorio no existe"));
                datos.id_territorio = validate.id_territorio;
                datos.activo = validate.activo;
                await new TerritorioRepository().Update(datos);
                return Responses.SetUpdatedResponse(datos);
            }
        }

        /// <summary>
        /// cambia estado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusCodeException"></exception>
        public async Task<Respuesta> cambiarTerritorio(int id)
        {
            using (var repo = new TerritorioRepository())
            {
                var validate = await repo.GetWithCondition(x => x.id_territorio == id);
                if (validate == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El territorio no existe"));
                validate.activo = !validate.activo;
                await repo.Update(validate);
                return Responses.SetUpdatedResponse(validate);
            }
        }
    }
}







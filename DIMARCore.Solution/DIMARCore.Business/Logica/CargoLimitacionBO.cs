using DIMARCore.Repositories;
using DIMARCore.Repositories.Repository;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DIMARCore.Business
{
    public class CargoLimitacionBO
    {

        /// <summary>
        /// Lista de Cargo Limitacion
        /// </summary>
        /// <returns>Lista de Cargo Limitacion</returns>
        /// <entidad>CARGO LIMITACION</entidad>
        /// <tabla>GENTEMAR_CARGO_LIMITACION</tabla>
        public IEnumerable<GENTEMAR_CARGO_LIMITACION> GetCargoLimitaciones()
        {
            // Obtiene la lista
            return new CargoLimitacionRepository().GetAll();
        }


        /// <summary>
        /// Obtener Cargo Limitacion dado su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objeto Cargo Limitacion dado su id</returns>
        /// <entidad>GENTEMAR_CARGO_LIMITACION</entidad>
        /// <tabla>GENTEMAR_CARGO_LIMITACION</tabla>
        public async Task<GENTEMAR_CARGO_LIMITACION> GetCargoLimitacion(int id)
        {
            return await new CargoLimitacionRepository().GetById(id);
        }


        /// <summary>
        /// Crea un Cargo Limitación
        /// </summary>
        /// <param name="datos">Información de un Cargo Limitación</param>
        /// <returns>Respuesta resultado</returns>
        public async Task<Respuesta> CrearCargoLimitacion(GENTEMAR_CARGO_LIMITACION datos)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                await new CargoLimitacionRepository().Create(datos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return respuesta;
        }


        /// <summary>
        /// Edita la información de un Cargo Limitación 
        /// </summary>
        /// <param name="datos">Datos de un Cargo Limitación</param>
        /// <param name="usuario">Usuario </param> //Todo: Se deja para implementar Autenticación
        /// <returns>Respuesta resultado</returns>
        public Respuesta EditarLimitación(GENTEMAR_LIMITACION datos)
        {

            Respuesta respuesta = new Respuesta();


            try
            {
                if (datos == null)
                {
                    respuesta = new Respuesta();

                    respuesta.Mensaje = $"Los datos para editar la Limitación  no son validos.";
                    respuesta.Estado = false;
                    respuesta.Data = null;
                    return respuesta;
                }

                //Limpia datos

                datos.limitaciones = string.IsNullOrEmpty(datos.limitaciones) ? null : datos.limitaciones.Trim().ToUpper();


                // busca la Limitación en el sistema

                GENTEMAR_LIMITACION limitacionActual = new LimitacionRepository().GetLimitacion(datos.id_limitacion);

                if (limitacionActual == null)
                {
                    //no se encontro la seccion
                    respuesta = new Respuesta();
                    respuesta.Mensaje = $"La Limitación con el id {datos.id_limitacion} no se encuentra en el sistema.";
                    respuesta.Estado = false;
                    respuesta.Data = null;
                    return respuesta;
                }

                //carga los datos de la Licencia que se pueden editar
                limitacionActual.limitaciones = datos.limitaciones;
                limitacionActual.activo = datos.activo;

                string mensaje = "";
                string mensajeIngles = "";
                //bool editar = new LimitacionRepository().Update(limitacionActual, out mensaje, out mensajeIngles);

                //if (editar)
                //{


                //    respuesta = new Respuesta();
                //    respuesta.Mensaje = mensaje;
                //    respuesta.MensajeIngles = mensajeIngles;
                //    respuesta.Estado = true;
                //    respuesta.Data = null;
                //    return respuesta;

                //}
                //else
                //{
                respuesta = new Respuesta();
                respuesta.Mensaje = mensaje;
                respuesta.MensajeIngles = mensajeIngles;
                respuesta.Estado = false;
                respuesta.Data = null;
                return respuesta;
                //}

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}







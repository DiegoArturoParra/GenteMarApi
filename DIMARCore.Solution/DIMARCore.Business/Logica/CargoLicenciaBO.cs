using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Config;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class CargoLicenciaBO
    {

        /// <summary>
        /// Lista de Cargo 
        /// </summary>
        /// <returns>Lista de Cargo </returns>
        /// <entidad>CARGO </entidad>
        /// <tabla>GENTEMAR_CARGO_LICENCIA</tabla>
        public IEnumerable<GENTEMAR_CARGO_LICENCIA> GetCargoLicencia(CargoLicenciaDTO filtro)
        {
            var data = new CargoLicenciaRepository().GetAllAsQueryable();
            // Obtiene la lista
            if (filtro.CargoLicencia != null)
            {
                data = data.Where(x => x.cargo_licencia.Contains(filtro.CargoLicencia));
            }
            else if (filtro.CodigoLicencia != null)
            {
                data = data.Where(x => x.codigo_licencia == filtro.CodigoLicencia);
            }

            return data.ToList();
        }


        /// <summary>
        /// Lista de Cargo licencia activo
        /// </summary>
        /// <returns>Lista de Cargo </returns>
        /// <entidad>CARGO </entidad>
        /// <tabla>GENTEMAR_CARGO_LICENCIA</tabla>
        public IEnumerable<CargoLicenciaDTO> CargoLicenciaActivo()
        {
            var claimIdCategoria = ClaimsHelper.ObtenerCategoriaUsuario();
            var data = new CargoLicenciaRepository().GetAllCargoLicenciaActivo(claimIdCategoria);
            // Obtiene la lista          

            return data;
        }

        /// <summary>
        /// Actializar  cargo licencia
        /// </summary>
        /// <returns>respuesta </returns>
        /// <entidad>CARGO </entidad>
        /// <tabla>GENTEMAR_CARGO_LICENCIA</tabla>

        public async Task<Respuesta> ActualizarAsync(GENTEMAR_CARGO_LICENCIA entidad)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (var repo = new CargoLicenciaRepository())
                {
                    var data = await repo.GetWithCondition(x => x.id_cargo_licencia == entidad.id_cargo_licencia);
                    if (data != null)
                    {
                        var actividadSeccion = await new ActividadSeccionLicenciasRepository().GetWithCondition(x => x.id_actividad == entidad.IdActividad && x.id_seccion == entidad.IdSeccion);
                        var seccionClase = await new SeccionClaseRepository().GetWithCondition(x => x.id_seccion == entidad.IdSeccion && x.id_clase == entidad.IdClase);
                        if (actividadSeccion != null)
                        {
                            if (seccionClase != null)
                            {
                                entidad.id_actividad_seccion_licencia = actividadSeccion.id_actividad_seccion_licencia;
                                entidad.id_seccion_clase = seccionClase.id_seccion_clase;
                                entidad.codigo_licencia = data.codigo_licencia;
                                entidad.activo = data.activo;
                                await new CargoLicenciaRepository().ModificarCargoLimitacion(entidad);
                                respuesta.StatusCode = HttpStatusCode.OK;
                                respuesta.Mensaje = ConstantesBO.EDITADO_OK;
                                respuesta.Estado = true;
                            }
                            else
                            {
                                respuesta.MensajeIngles = "User already registered";
                                respuesta.StatusCode = HttpStatusCode.Conflict;
                                respuesta.Mensaje = "La relación de sección y clase no existe.";
                                respuesta.Estado = false;
                            }
                        }
                        else
                        {
                            respuesta.MensajeIngles = "User already registered";
                            respuesta.StatusCode = HttpStatusCode.Conflict;
                            respuesta.Mensaje = "La relación de actividad y sección no existe.";
                            respuesta.Estado = false;
                        }
                    }
                    else
                    {
                        respuesta.MensajeIngles = "User already registered";
                        respuesta.StatusCode = HttpStatusCode.Conflict;
                        respuesta.Mensaje = $"El cargo licencia {entidad.cargo_licencia} ya se encuentra registrado.";
                        respuesta.Estado = false;
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta.StatusCode = HttpStatusCode.InternalServerError;
                respuesta.MensajeExcepcion = ex.Message;
                respuesta.Mensaje = string.Empty;
            }

            return respuesta;
        }

        /// <summary>
        /// Crear cargo licencia
        /// </summary>
        /// <returns>respuesta </returns>
        /// <entidad>CARGO </entidad>
        /// <tabla>GENTEMAR_CARGO_LICENCIA</tabla>

        public async Task<Respuesta> CrearAsync(GENTEMAR_CARGO_LICENCIA entidad)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (var repo = new CargoLicenciaRepository())
                {
                    var data = await repo.GetWithCondition(x => x.cargo_licencia == entidad.cargo_licencia);
                    if (data == null)
                    {
                        var actividadSeccion = await new ActividadSeccionLicenciasRepository().GetWithCondition(x => x.id_actividad == entidad.IdActividad && x.id_seccion == entidad.IdSeccion);
                        var seccionClase = await new SeccionClaseRepository().GetWithCondition(x => x.id_seccion == entidad.IdSeccion && x.id_clase == entidad.IdClase);
                        if (actividadSeccion != null)
                        {
                            if (seccionClase != null)
                            {
                                entidad.id_actividad_seccion_licencia = actividadSeccion.id_actividad_seccion_licencia;
                                entidad.id_seccion_clase = seccionClase.id_seccion_clase;
                                await repo.CrearCargoLimitacion(entidad);
                                entidad.codigo_licencia = await codigoLicenciaAsync(entidad, 0);
                                await new CargoLicenciaRepository().Update(entidad);
                                respuesta.StatusCode = HttpStatusCode.Created;
                                respuesta.Mensaje = ConstantesBO.CREADO_OK;
                                respuesta.Estado = true;
                            }
                            else
                            {
                                respuesta.MensajeIngles = "User already registered";
                                respuesta.StatusCode = HttpStatusCode.Conflict;
                                respuesta.Mensaje = "La relacion de seccion y clase no existe";
                            }
                        }
                        else
                        {
                            respuesta.MensajeIngles = "User already registered";
                            respuesta.StatusCode = HttpStatusCode.Conflict;
                            respuesta.Mensaje = "La relacion de actividad y seccion no existe";
                        }
                    }
                    else
                    {
                        respuesta.MensajeIngles = "User already registered";
                        respuesta.StatusCode = HttpStatusCode.Conflict;
                        respuesta.Mensaje = $"El cargo licencia {entidad.cargo_licencia} ya se encuentra registrado.";
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta.StatusCode = HttpStatusCode.InternalServerError;
                respuesta.MensajeExcepcion = ex.Message;
                respuesta.Mensaje = string.Empty;
            }

            return respuesta;
        }


        /// <summary>
        /// codigo licencia 
        /// </summary>
        /// <returns>codigo licencia generado  </returns>
        /// <entidad>CARGO </entidad>
        /// <tabla>GENTEMAR_CARGO_LICENCIA</tabla>
        private async Task<string> codigoLicenciaAsync(GENTEMAR_CARGO_LICENCIA licencia, int fecha)
        {
            var codigo = "";
            if (fecha == 0)
            {
                fecha = DateTime.Now.Year;
            }
            else
            {
                fecha = fecha + 1;
            }
            codigo = $"{licencia.cargo_licencia.Substring(0, 1)}{fecha}{licencia.id_cargo_licencia}";
            var data = await new CargoLicenciaRepository().AnyWithCondition(x => x.codigo_licencia == codigo);
            if (data)
            {
                codigo = await codigoLicenciaAsync(licencia, fecha);
            }

            return codigo;
        }

        /// <summary>
        /// Lista de cargo licencia id
        /// </summary>
        /// <returns>Lista de Cargo </returns>
        /// <entidad>CARGO </entidad>
        /// <tabla>GENTEMAR_CARGO_LICENCIA</tabla>
        public CargoLicenciaDTO GetCargoLicenciaId(long id)
        {
            var data = new CargoLicenciaRepository().GetCargoLicenciaId(id);
            if (data == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encontro la licencia solicitada");
            }
            // Obtiene la lista
            return data;
        }
        /// <summary>
        /// Lista de cargo licencia id detalle
        /// </summary>
        /// <returns>Lista de Cargo </returns>
        /// <entidad>CARGO </entidad>
        /// <tabla>GENTEMAR_CARGO_LICENCIA</tabla>
        public CargoLicenciaDTO GetCargoLicenciaIdDetalle(long id)
        {
            var data = new CargoLicenciaRepository().GetCargoLicenciaIdDetalle(id);
            // Obtiene la lista
            return data;
        }


        /// <summary>
        /// cambia el estado del rango 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Respuesta> cambiarCargoLicencia(int id)
        {
            Respuesta respuesta = new Respuesta();
            using (var repo = new CargoLicenciaRepository())
            {
                var validate = await repo.GetWithCondition(x => x.id_cargo_licencia == id);
                if (validate != null)
                {
                    validate.activo = !validate.activo;
                    await repo.Update(validate);
                    respuesta.StatusCode = HttpStatusCode.Created;
                    respuesta.Mensaje = ConstantesBO.EDITADO_OK;
                    return respuesta;
                }
                respuesta.StatusCode = HttpStatusCode.Conflict;
                respuesta.Mensaje = "El tipo de cargo licencia no existe";
                respuesta.Estado = false;
                return respuesta;
            }
        }



    }
}

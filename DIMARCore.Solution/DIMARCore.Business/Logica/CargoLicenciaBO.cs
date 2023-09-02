using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Config;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
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

        private CargoLicenciaRepository _repository;

        public CargoLicenciaBO()
        {
            _repository = new CargoLicenciaRepository();
        }


        /// <summary>
        /// Lista de Cargo 
        /// </summary>
        /// <returns>Lista de Cargo </returns>
        /// <entidad>CARGO </entidad>
        /// <tabla>GENTEMAR_CARGO_LICENCIA</tabla>
        public IEnumerable<GENTEMAR_CARGO_LICENCIA> GetCargoLicencia(CargoLicenciaDTO filtro)
        {
            var data = _repository.GetAllAsQueryable();
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
            var claimIdCategoria = ClaimsHelper.GetCategoriaUsuario();
            var data = _repository.GetAllCargoLicenciaActivo(claimIdCategoria);
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
                using (var repo = _repository)
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
                                data.cargo_licencia = entidad.cargo_licencia;
                                data.vigencia = entidad.vigencia;
                                data.IdTipoLicencia = entidad.IdTipoLicencia;
                                data.IdActividad = entidad.IdActividad;
                                data.IdSeccion = entidad.IdSeccion;
                                data.IdClase = entidad.IdClase;
                                data.id_actividad_seccion_licencia = actividadSeccion.id_actividad_seccion_licencia;
                                data.id_seccion_clase = seccionClase.id_seccion_clase;
                                data.IdCategoria = entidad.IdCategoria;
                                data.IdLimitacion = entidad.IdLimitacion;
                                data.IdLimitante = entidad.IdLimitante;
                                await _repository.ModificarCargoLimitacion(data);
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
                using (var repo = _repository)
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
                                await _repository.Update(entidad);
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
            var data = await _repository.AnyWithCondition(x => x.codigo_licencia == codigo);
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
            var data = _repository.GetCargoLicenciaId(id)
                ?? throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encontro la licencia solicitada");
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
            var data = _repository.GetCargoLicenciaIdDetalle(id);
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
            using (var repo = _repository)
            {
                var validate = await repo.GetWithCondition(x => x.id_cargo_licencia == id)
                    ?? throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encontro la licencia solicitada");
                validate.activo = !validate.activo;
                await repo.Update(validate);
                return Responses.SetUpdatedResponse(validate);
            }
        }

        /// <summary>
        /// metodo para obtener los datos del usuario y de la licencia para la plantilla
        /// por id de la licencia.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PlantillaLicenciaDTO> GetPlantillaLicencias(long id)
        {
            var data = await _repository.GetPlantillaLicencias(id);

            return data;
        }
    }
}

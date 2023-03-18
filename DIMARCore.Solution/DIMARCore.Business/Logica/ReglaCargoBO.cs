using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class ReglaCargoBO
    {
        public async Task<Respuesta> GetIdByTablasForaneas(IdsTablasForaneasDTO idsTablas)
        {
            var CargoReglaId = await new ReglaCargoRepository().GetIdReglaCargo(idsTablas);
            if (CargoReglaId == 0)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encuentra registrado el id cargo regla."));
            return Responses.SetOkResponse(CargoReglaId);
        }


        public async Task<Respuesta> ExisteCargoTituloInDetalleRegla(int cargoId)
        {
            var HayCargoTituloInDetalleReglas = await new ReglaCargoRepository().ExisteCargoTituloInDetalleRegla(cargoId);
            if (!HayCargoTituloInDetalleReglas)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("Todavia no hay reglas por el cargo del titulo digitado."));
            return Responses.SetOkResponse();
        }

        public async Task<Respuesta> GetById(int id)
        {
            Respuesta response = new Respuesta();
            try
            {
                using (var repo = new ReglaCargoRepository())
                {
                    var existe = await repo.AnyWithCondition(x => x.id_cargo_regla == id);
                    if (existe)
                    {
                        response.Data = await repo.GetDetalleById(id);
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response.Mensaje = "No existe la relación.";
                        response.StatusCode = HttpStatusCode.NotFound;
                        response.MensajeIngles = "Not found.";
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.MensajeExcepcion = ex.Message;
                Debug.WriteLine(ex.Message);
            }
            return response;
        }

        public IEnumerable<ListadoDetalleCargoReglaDTO> GetListado(DetalleReglaFilter filtro)
        {
            return new ReglaCargoRepository().GetListado(filtro);
        }

        public async Task<Respuesta> CrearCargoRegla(GENTEMAR_REGLAS_CARGO data)
        {
            Respuesta response = new Respuesta();
            try
            {
                var validaciones = await ValidarFormularioAsync(data);
                if (validaciones.Valido)
                {
                    await new ReglaCargoRepository().CrearRelacionReglaCargo(data);
                    response.Mensaje = ConstantesBO.CREADO_OK;
                    response.StatusCode = HttpStatusCode.Created;
                    response.Estado = true;
                    response.MensajeIngles = ConstantesBO.OK;
                }
                else
                {
                    response.Mensaje = validaciones.Mensaje;
                    response.StatusCode = validaciones.status;
                    response.Estado = validaciones.Valido;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.MensajeExcepcion = ex.Message;
                response.Mensaje = string.Empty;
                Debug.WriteLine(ex.Message);
            }
            return response;
        }
        public async Task<Respuesta> EditarCargoRegla(GENTEMAR_REGLAS_CARGO data)
        {
            Respuesta response = new Respuesta();
            try
            {

                var validaciones = await ValidarFormularioAsync(data, true);
                if (validaciones.Valido)
                {
                    using (var repo = new ReglaCargoRepository())
                    {
                        var entidad = await repo.GetWithCondition(x => x.id_cargo_regla == data.id_cargo_regla);
                        if (entidad != null)
                        {
                            entidad.Habilitaciones = data.Habilitaciones;
                            entidad.id_nivel = data.id_nivel;
                            entidad.id_capacidad = data.id_capacidad;
                            await repo.ActualizarRelacionReglaCargo(entidad);
                            response.Mensaje = ConstantesBO.EDITADO_OK;
                            response.StatusCode = HttpStatusCode.OK;
                            response.Estado = true;
                            response.MensajeIngles = ConstantesBO.OK;
                        }
                        else
                        {
                            response.Mensaje = "No se encontro el objeto.";
                            response.StatusCode = HttpStatusCode.NotFound;
                            response.Estado = false;
                            response.Data = null;
                        }
                    }
                }
                else
                {
                    response.Mensaje = validaciones.Mensaje;
                    response.StatusCode = validaciones.status;
                    response.Estado = validaciones.Valido;
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.MensajeExcepcion = ex.Message;
                response.Mensaje = string.Empty;
                response.Data = null;
                Debug.WriteLine(ex.Message);
            }
            return response;
        }

        private async Task<(string Mensaje, bool Valido, HttpStatusCode status)> ValidarFormularioAsync(GENTEMAR_REGLAS_CARGO entidad, bool Update = false)
        {
            bool valido = true;
            bool existeRelacion = false;
            string mensaje = string.Empty;
            HttpStatusCode httpStatusCode = HttpStatusCode.OK;
            if (entidad.id_cargo_regla > 0)
            {
                var existeCargoRegla = await new ReglaCargoRepository().AnyWithCondition(x => x.id_cargo_regla == entidad.id_cargo_regla);
                if (!existeCargoRegla)
                {
                    valido = false;
                    mensaje = $"No existe el id de la tabla.";
                    httpStatusCode = HttpStatusCode.NotFound;
                }
            }

            var existeNivel = await new NivelTituloRepository().AnyWithCondition(x => x.id_nivel == entidad.id_nivel);
            if (!existeNivel)
            {
                valido = false;
                mensaje = $"No existe el nivel digitado.";
                httpStatusCode = HttpStatusCode.NotFound;
            }

            var cargoTitulo = await new CargoTituloRepository().AnyWithCondition(x => x.id_cargo_titulo == entidad.id_cargo_titulo);
            if (!cargoTitulo)
            {
                valido = false;
                mensaje = $"No existe el cargo del titulo digitado.";
                httpStatusCode = HttpStatusCode.NotFound;
            }
            var existeRegla = await new ReglaRepository().AnyWithCondition(x => x.id_regla == entidad.id_regla);
            if (!existeRegla)
            {
                valido = false;
                mensaje = $"No existe la regla digitada.";
                httpStatusCode = HttpStatusCode.NotFound;
            }

            var existeHabilitaciones = await ExisteHabilitaciones(entidad.Habilitaciones);
            if (!existeHabilitaciones)
            {
                valido = false;
                mensaje = $"No existe la habilitación digitada.";
                httpStatusCode = HttpStatusCode.NotFound;
            }

            if (!Update)
            {
                existeRelacion = await new ReglaCargoRepository().AnyWithCondition(x => x.id_regla == entidad.id_regla && x.id_nivel == entidad.id_nivel
                                 && x.id_cargo_titulo == entidad.id_cargo_titulo && x.id_capacidad == entidad.id_capacidad);
            }
            if (existeRelacion)
            {
                valido = false;
                mensaje = $"La relación ya existe no se puede insertar.";
                httpStatusCode = HttpStatusCode.Conflict;
            }

            return (mensaje, valido, httpStatusCode);
        }

        private async Task<bool> ExisteHabilitaciones(List<int> habilitacionesId)
        {
            bool existe = true;
            foreach (var item in habilitacionesId)
            {
                existe = await new HabilitacionRepository().AnyWithCondition(x => x.id_habilitacion == item);
            }
            return existe;
        }
    }
}

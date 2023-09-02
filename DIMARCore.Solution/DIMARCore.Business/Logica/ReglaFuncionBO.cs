using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
namespace DIMARCore.Business.Logica
{
    public class ReglaFuncionBO
    {

        public async Task<IEnumerable<InfoReglaFuncionDTO>> GetAll()
        {
            return await new ReglaFuncionRepository().GetDetalles();
        }

        public async Task<Respuesta> CrearAsync(ReglaFuncionDTO entidad)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await Validaciones(entidad);
                if (respuesta.Estado)
                {
                    using (var repo = new ReglaFuncionRepository())
                    {
                        List<GENTEMAR_REGLA_FUNCION> entidades = (List<GENTEMAR_REGLA_FUNCION>)respuesta.Data;
                        await repo.CreateInCascade(entidades);
                        respuesta.StatusCode = HttpStatusCode.Created;
                        respuesta.Mensaje = ConstantesBO.CREADO_OK;
                        respuesta.Estado = true;
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta.Data = null;
                respuesta.StatusCode = HttpStatusCode.InternalServerError;
                respuesta.MensajeExcepcion = ex.Message;
                respuesta.Mensaje = string.Empty;
                Debug.WriteLine(ex.Message);
            }

            return respuesta;
        }

        public async Task<Respuesta> ActualizarAsync(ReglaFuncionDTO entidad)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (var repo = new ReglaFuncionRepository())
                {
                    respuesta = await Validaciones(entidad, true);
                    if (respuesta.Estado)
                    {
                        await repo.UpdateInCascade((List<GENTEMAR_REGLA_FUNCION>)respuesta.Data,
                            entidad.ReglaId);
                        respuesta.StatusCode = HttpStatusCode.OK;
                        respuesta.Mensaje = ConstantesBO.EDITADO_OK;
                        respuesta.Estado = true;
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta.StatusCode = HttpStatusCode.InternalServerError;
                respuesta.MensajeExcepcion = ex.Message;
                respuesta.Mensaje = string.Empty;
                Debug.WriteLine(ex.Message);
            }

            return respuesta;
        }

        public async Task<IEnumerable<ReglaDTO>> ReglasSinFunciones()
        {
            return await new ReglaFuncionRepository().GetReglasSinFunciones();
        }

        public async Task<Respuesta> Validaciones(ReglaFuncionDTO entidad, bool update = false)
        {
            bool existeRelacion = false;
            Respuesta response = new Respuesta();
            List<GENTEMAR_REGLA_FUNCION> entidades = new List<GENTEMAR_REGLA_FUNCION>();
            response = await new ReglaBO().ExisteReglaById(entidad.ReglaId);
            if (response.Estado)
            {
                foreach (var item in entidad.Funciones)
                {
                    var existeFuncion = await new FuncionRepository().AnyWithCondition(y => y.id_funcion == item);
                    if (!existeFuncion)
                    {
                        response.Estado = false;
                        response.StatusCode = HttpStatusCode.NotFound;
                        response.MensajeIngles = "Not found.";
                        response.Mensaje = "No existe la función indicada.";
                        break;
                    }
                    if (!update)
                    {
                        existeRelacion = await new ReglaFuncionRepository().AnyWithCondition(x => x.id_regla == entidad.ReglaId && x.id_funcion == item);
                    }
                    if (existeRelacion)
                    {
                        response.Estado = false;
                        response.StatusCode = HttpStatusCode.Conflict;
                        response.MensajeIngles = "Conflict.";
                        response.Mensaje = "Ya existe la relación indicada, ingrese una valida.";
                        break;
                    }
                    else
                    {
                        entidades.Add(new GENTEMAR_REGLA_FUNCION()
                        {
                            id_regla = entidad.ReglaId,
                            id_funcion = item
                        });
                    }
                }
                if (response.Estado)
                {
                    response.Mensaje = ConstantesBO.OK;
                    response.Data = entidades;
                    response.StatusCode = HttpStatusCode.OK;
                }
            }
            return response;
        }
    }
}

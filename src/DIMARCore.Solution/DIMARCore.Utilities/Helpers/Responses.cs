using log4net;
using Newtonsoft.Json;
using System;
using System.Net;

namespace DIMARCore.Utilities.Helpers
{
    public class Responses
    {
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static Respuesta SetOkResponse(Object pData = null, string Mensaje = "Ok")
        {
            Respuesta pResponse = new Respuesta
            {
                StatusCode = HttpStatusCode.OK,
                Mensaje = Mensaje,
                MensajeIngles = "Request OK.",
                Estado = true
            };
            if (pData != null && !String.IsNullOrEmpty(pData.ToString()))
            {
                pResponse.Data = pData;
            }
            return pResponse;
        }

        public static Respuesta SetCreatedResponse(Object pData = null, string mensaje = "Se ha creado satisfactoriamente.")
        {
            Respuesta pResponse = new Respuesta
            {
                StatusCode = HttpStatusCode.Created,
                Mensaje = mensaje,
                MensajeIngles = "Successfully created.",
                Estado = true
            };
            if (pData != null && !String.IsNullOrEmpty(pData.ToString()))
            {
                pResponse.Data = pData;
            }
            return pResponse;
        }

        public static Respuesta SetUpdatedResponse(Object pData = null)
        {
            Respuesta pResponse = new Respuesta
            {
                StatusCode = HttpStatusCode.OK,
                Mensaje = "Se ha editado satisfactoriamente.",
                MensajeIngles = "Successfully edited.",
                Estado = true
            };
            if (pData != null && !String.IsNullOrEmpty(pData.ToString()))
            {
                pResponse.Data = pData;
            }
            return pResponse;
        }

        public static Respuesta SetAcceptedResponse(Object pData = null, string mensaje = "Se ha aceptado satisfactoriamente.")
        {
            Respuesta pResponse = new Respuesta
            {
                StatusCode = HttpStatusCode.Accepted,
                Mensaje = mensaje,
                MensajeIngles = "request has been accepted for processing."
            };
            if (pData != null && !String.IsNullOrEmpty(pData.ToString()))
            {
                pResponse.Data = pData;
            }
            pResponse.Estado = true;
            return pResponse;
        }

        public static Respuesta SetBadRequestResponse(string Mensaje, Object pData = null)
        {
            Respuesta pResponse = new Respuesta
            {
                StatusCode = HttpStatusCode.BadRequest,
                Mensaje = Mensaje,
                MensajeIngles = "Bad Request.",
                Estado = false
            };
            if (pData != null && !String.IsNullOrEmpty(pData.ToString()))
            {
                pResponse.Data = pData;
            }
            string json = JsonConvert.SerializeObject(pResponse);
            _logger.Warn(json);
            return pResponse;
        }


        public static Respuesta SetNotFoundResponse(string mensaje)
        {
            Respuesta pResponse = new Respuesta
            {
                StatusCode = HttpStatusCode.NotFound,
                Mensaje = mensaje,
                MensajeIngles = "Resource not found.",
                Estado = false
            };
            return pResponse;
        }

        public static Respuesta SetUnathorizedResponse(string mensaje)
        {
            Respuesta pResponse = new Respuesta
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Mensaje = mensaje,
                MensajeIngles = "Access denied.",
                Estado = false
            };
            return pResponse;
        }
        public static Respuesta SetConflictResponse(string mensaje)
        {
            Respuesta pResponse = new Respuesta
            {
                StatusCode = HttpStatusCode.Conflict,
                Mensaje = mensaje,
                MensajeIngles = "Target resource request conflict.",
                Estado = false
            };
            return pResponse;
        }

        public static Respuesta SetRequestCanceledResponse(OperationCanceledException exception, string mensaje)
        {
            Respuesta pResponse = new Respuesta
            {
                StatusCode = HttpStatusCode.RequestTimeout,
                Mensaje = mensaje,
                MensajeExcepcion = exception.InnerException != null ? exception.InnerException.Message : exception.Message,
                Data = exception.StackTrace,
                MensajeIngles = "The operation has been canceled.",
                Estado = false
            };
            return pResponse;
        }
        public static Respuesta SetInternalServerErrorResponse(Exception exception, string mensaje = "Internal Server Error")
        {
            Respuesta pResponse = new Respuesta
            {
                StatusCode = HttpStatusCode.InternalServerError,
                MensajeExcepcion = exception.InnerException != null ? exception.InnerException.Message : exception.Message,
                Data = exception.StackTrace,
                Mensaje = mensaje,
                MensajeIngles = "HTTP 500 Internal Server Error",
                Estado = false
            };
            string json = JsonConvert.SerializeObject(pResponse);
            _logger.Error(json);
            return pResponse;
        }
    }
}

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
            Respuesta pResponse = new Respuesta();
            pResponse.StatusCode = HttpStatusCode.OK;
            pResponse.Mensaje = Mensaje;
            pResponse.MensajeIngles = "Request OK.";
            if (pData != null && !String.IsNullOrEmpty(pData.ToString()))
            {
                pResponse.Data = pData;
            }
            pResponse.Estado = true;
            return pResponse;
        }

        public static Respuesta SetInternalServerErrorResponse(Exception exception, string mensaje = "Internal Server Error")
        {
            Respuesta pResponse = new Respuesta();
            pResponse.StatusCode = HttpStatusCode.InternalServerError;
            pResponse.MensajeExcepcion = exception.Message;
            pResponse.Data = exception.InnerException;
            pResponse.Mensaje = $"{mensaje} {exception.Message}";
            pResponse.MensajeIngles = "HTTP 500 Internal Server Error";
            pResponse.Estado = false;
            string json = JsonConvert.SerializeObject(pResponse);
            _logger.Error(json);
            return pResponse;
        }

        public static Respuesta SetCreatedResponse(Object pData = null)
        {
            Respuesta pResponse = new Respuesta();
            pResponse.StatusCode = HttpStatusCode.Accepted;
            pResponse.Mensaje = "Se ha creado satisfactoriamente.";
            pResponse.MensajeIngles = "Successfully created.";
            if (pData != null && !String.IsNullOrEmpty(pData.ToString()))
            {
                pResponse.Data = pData;
            }
            pResponse.Estado = true;
            return pResponse;
        }

        public static Respuesta SetAcceptedResponse(Object pData = null, string mensaje = "Se ha aceptado satisfactoriamente.")
        {
            Respuesta pResponse = new Respuesta();
            pResponse.StatusCode = HttpStatusCode.Accepted;
            pResponse.Mensaje = mensaje;
            pResponse.MensajeIngles = "request has been accepted for processing.";
            if (pData != null && !String.IsNullOrEmpty(pData.ToString()))
            {
                pResponse.Data = pData;
            }
            pResponse.Estado = true;
            return pResponse;
        }

        public static Respuesta SetNotFoundResponse(string mensaje)
        {
            Respuesta pResponse = new Respuesta();
            pResponse.StatusCode = HttpStatusCode.NotFound;
            pResponse.Mensaje = mensaje;
            pResponse.MensajeIngles = "Resource not found.";
            pResponse.Estado = false;
            return pResponse;
        }

        public static Respuesta SetUnathorizedResponse(string mensaje)
        {
            Respuesta pResponse = new Respuesta();
            pResponse.StatusCode = HttpStatusCode.Unauthorized;
            pResponse.Mensaje = mensaje;
            pResponse.MensajeIngles = "Access denied.";
            pResponse.Estado = false;
            return pResponse;
        }
        public static Respuesta SetConflictResponse(string mensaje)
        {
            Respuesta pResponse = new Respuesta();
            pResponse.StatusCode = HttpStatusCode.Conflict;
            pResponse.Mensaje = mensaje;
            pResponse.MensajeIngles = "target resource request conflict.";
            pResponse.Estado = false;
            return pResponse;
        }
        public static Respuesta SetUpdatedResponse(Object pData = null)
        {
            Respuesta pResponse = new Respuesta();
            pResponse.StatusCode = HttpStatusCode.OK;
            pResponse.Mensaje = "Se ha editado satisfactoriamente.";
            pResponse.MensajeIngles = "Successfully edited.";
            if (pData != null && !String.IsNullOrEmpty(pData.ToString()))
            {
                pResponse.Data = pData;
            }
            pResponse.Estado = true;
            return pResponse;
        }
    }
}

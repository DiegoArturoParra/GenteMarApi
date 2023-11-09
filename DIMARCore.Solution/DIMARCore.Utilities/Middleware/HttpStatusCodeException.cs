using DIMARCore.Utilities.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;

namespace DIMARCore.Utilities.Middleware
{
    /// <summary>
    /// CLase para el middleware
    /// </summary>
    public class HttpStatusCodeException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }
        public string StackTraced { get; set; }
        public string MessageException { get; set; }
        public string MessageEnglish { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ContentType { get; set; } = @"text/plain";

        /// <summary>
        /// constructor que recibe un parametro el tipo de response.
        /// </summary>
        /// <param name="statusCode"></param>
        public HttpStatusCodeException(HttpStatusCode statusCode)
        {
            this.StatusCode = statusCode;
        }

        /// <summary>
        /// constructor que recibe un objeto de tipo respuesta
        /// </summary>
        /// <param name="response"></param>
        public HttpStatusCodeException(Respuesta response)
           : base(response.Mensaje)
        {
            this.StatusCode = response.StatusCode;
            this.MessageException = response.MensajeExcepcion;
            this.MessageEnglish = response.MensajeIngles;
            this.StackTraced = JsonConvert.SerializeObject(response.Data);
        }

        /// <summary>
        /// constructor con el parametro statusCode y mensaje
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        public HttpStatusCodeException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            this.StatusCode = statusCode;
        }
        /// <summary>
        /// constructor con inner exception
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="inner"></param>
        public HttpStatusCodeException(HttpStatusCode statusCode, Exception inner)
            : this(statusCode, inner.ToString()) { }


        /// <summary>
        /// constructor con objeto de errores
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="errorObject"></param>
        public HttpStatusCodeException(HttpStatusCode statusCode, JObject errorObject)
            : this(statusCode, errorObject.ToString())
        {
            this.ContentType = @"application/json";
        }
    }
}
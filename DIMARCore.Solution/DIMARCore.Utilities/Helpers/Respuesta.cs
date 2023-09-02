using System;
using System.Net;

namespace DIMARCore.Utilities.Helpers
{
    public class Respuesta
    {
        /// <summary>
        /// atributo que da el estado de la solicitud 
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }
        /// <summary>
        /// Código del resultado
        /// </summary>
        public int Resultado => ResultadoBinario();

        /// <summary>
        /// Mensaje 
        /// </summary>
        public string Mensaje { get; set; }
        /// <summary>
        /// Mensaje Ingles
        /// </summary>
        public string MensajeIngles { get; set; }
        /// <summary>
        /// Mensaje excepcion
        /// </summary>
        public string MensajeExcepcion { get; set; }
        /// <summary>
        /// Objeto de respuesta
        /// </summary>
        public Object Data { get; set; }
        /// <summary>
        /// Estado
        /// </summary>
        public bool Estado { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Respuesta()
        {
            Mensaje = string.Empty;
            MensajeIngles = string.Empty;
            MensajeExcepcion = string.Empty;
            Data = null;
            Estado = false;
        }

        private int ResultadoBinario()
        {
            if ((int)this.StatusCode >= 400 && (int)this.StatusCode < 500)
            {
                return 0;
            }
            else if ((int)this.StatusCode >= 500)
            {
                return -1;
            }
            return 1;
        }
    }
}

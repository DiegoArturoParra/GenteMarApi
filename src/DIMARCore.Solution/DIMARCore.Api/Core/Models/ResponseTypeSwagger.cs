using System.Net;

namespace DIMARCore.Api.Core.Models
{
    /// <summary>
    /// Clase que representa la respuesta de una solicitud
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseTypeSwagger<T>
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
        public T Data { get; set; }
        /// <summary>
        /// Estado
        /// </summary>
        public bool Estado { get; set; }


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
    /// <summary>
    /// response de editar 
    /// </summary>
    public class ResponseEditTypeSwagger
    {
        /// <summary>
        /// status de la solicitud
        /// </summary>
        public HttpStatusCode StatusCode => HttpStatusCode.OK;
        /// <summary>
        /// mensaje en español
        /// </summary>
        public string Mensaje => "Se ha editado satisfactoriamente.";
        /// <summary>
        /// mensaje en ingles
        /// </summary>
        public string MensajeIngles => "Successfully edited.";
        /// <summary>
        /// estado de la solicitud
        /// </summary>
        public bool Estado => true;
        /// <summary>
        /// Código del resultado
        /// </summary>
        public int Resultado => 1;

    }
    /// <summary>
    /// response de editar 
    /// </summary>
    public class ResponseCreatedTypeSwagger
    {
        /// <summary>
        /// status de la solicitud
        /// </summary>
        public HttpStatusCode StatusCode => HttpStatusCode.Created;
        /// <summary>
        /// mensaje en español
        /// </summary>
        public string Mensaje => "Se ha creado satisfactoriamente.";
        /// <summary>
        /// mensaje en ingles
        /// </summary>
        public string MensajeIngles => "Successfully created.";
        /// <summary>
        /// estado de la solicitud
        /// </summary>
        public bool Estado => true;
        /// <summary>
        /// Código del resultado
        /// </summary>
        public int Resultado => 1;

    }
}
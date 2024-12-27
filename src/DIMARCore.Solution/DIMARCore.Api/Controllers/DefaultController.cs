using System;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DIMARCore.Api.Controllers
{
    /// <summary>
    /// servicios genero
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/default")]
    public class DefaultController : ApiController
    {
        /// <summary>
        /// Servicio para traer la fecha y hora actual y medir el tiempo de respuesta
        /// </summary>
        /// <returns></returns>
        [Route("datetime")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetDateTimeWithElapsedTime()
        {
            // Iniciar cronómetro
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Obtener la fecha y hora actual
            var currentDateTime = DateTime.Now;

            // Detener cronómetro
            stopwatch.Stop();

            // Retornar la fecha y hora junto con el tiempo transcurrido
            var response = new
            {
                CurrentDateTime = currentDateTime,
                ElapsedTimeInMilliseconds = stopwatch.ElapsedMilliseconds
            };

            return Ok(response);
        }
    }
}

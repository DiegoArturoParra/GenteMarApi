using DIMARCore.Api.Core;
using DIMARCore.Api.Core.Filters;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DIMARCore.Api
{
    /// <summary>
    /// 
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {

            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
            config.MessageHandlers.Add(new TokenValidationHandler());
            config.Filters.Add(new AuthorizeAttribute());
            config.Filters.Add(new ValidationExceptionFilterAttribute());
            config.Filters.Add(new CustomExceptionFilterAttribute());
  
            // se elimina el formateador de respuestas xml
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            // se quita el fomato xml
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // se habilita el formateador de respuestas json
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;

            // se define que los json de respuesta sean indentados
            config.Formatters.JsonFormatter.Indent = true;
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("multipart/form-data"));
            // Rutas de API web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

        }
    }
}

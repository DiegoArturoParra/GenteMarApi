using DIMARCore.Api;
using Swashbuckle.Application;
using Swashbuckle.Swagger;
using System.Collections.Generic;
using System.IO;
using System.Web.Http;
using System.Web.Http.Description;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace DIMARCore.Api
{
    /// <summary>
    /// Clase para configurar Swagger y generar documentación de API
    /// </summary>
    public class SwaggerConfig
    {
        /// <summary>
        /// OBTENEMOS EL PATH DEL ARCHIVO XML DE DOCUMENTACIÓN.
        /// </summary>
        /// <returns></returns>
        protected static string GetXmlCommentsPath()
        {
            return Path.Combine(System.Web.HttpRuntime.AppDomainAppPath, "App_Data", "GDMCore.Api.xml");
        }

        /// <summary>
        /// registro de api con swagger 
        /// </summary>
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("V1", "Api GENTE DE MAR con Token JWT");
                    c.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
                    c.IncludeXmlComments(GetXmlCommentsPath());
                    c.PrettyPrint();
                })
                .EnableSwaggerUi();


        }

        /// <summary>
        /// AuthorizationHeaderParameterOperationFilter para introducir JWT en dialogo Swagger
        /// </summary>
        public class AuthorizationHeaderParameterOperationFilter : IOperationFilter
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="operation"></param>
            /// <param name="schemaRegistry"></param>
            /// <param name="apiDescription"></param>
            public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
            {
                if (operation.parameters == null)
                    operation.parameters = new List<Parameter>();

                operation.parameters.Add(new Parameter
                {
                    name = "Authorization",
                    @in = "header",
                    description = "JWT Token",
                    required = false,
                    type = "string"
                });
            }
        }
    }
}

using System.Web.Http;
using WebActivatorEx;
using DIMARCore.Api;
using Swashbuckle.Swagger;
using Swashbuckle.Application;
using System.Collections.Generic;
using System.Web.Http.Description;
using System.IO;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace DIMARCore.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class SwaggerConfig
    {
        /// <summary>
        /// OBTENEMOS EL PATH DEL ARCHIVO XML DE DOCUMENTACIÓN.
        /// </summary>
        /// <returns></returns>
        protected static string GetXmlCommentsPath()
        {
            return Path.Combine(System.Web.HttpRuntime.AppDomainAppPath, "bin", "DIMARCore.Api.xml");
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
                    // If you annotate Controllers and API Types with
                    // Xml comments (http://msdn.microsoft.com/en-us/library/b2s063f7(v=vs.110).aspx), you can incorporate
                    // those comments into the generated docs and UI. You can enable this by providing the path to one or
                    // more Xml comment files.
                    //
                    // HABILITAMOS EL ARCHIVO DE DOCUMENTACIÓN XML.
                    c.IncludeXmlComments(GetXmlCommentsPath());

                    // If you want the output Swagger docs to be indented properly, enable the "PrettyPrint" option.
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

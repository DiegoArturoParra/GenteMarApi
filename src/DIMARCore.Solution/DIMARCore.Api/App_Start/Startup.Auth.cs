using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(DIMARCore.Api.App_Start.Startup))]

namespace DIMARCore.Api.App_Start
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            // Para obtener más información sobre cómo configurar la aplicación, visite https://go.microsoft.com/fwlink/?LinkID=316888
        }

    }
}
using System.Web.Http;
using System.Web.Mvc;

namespace DIMARCore.Api.Areas.HelpPage
{
    public class HelpPageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "HelpPage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //context.MapRoute(
            //    "HelpPage_Default",
            //    "Help/{action}/{apiId}",
            //    new { controller = "Help", action = "Index", apiId = UrlParameter.Optional });

            //HelpPageConfig.Register(GlobalConfiguration.Configuration);

            context.MapRoute(
                "HelpPage_Default",
                "documentacion-dimar-api/{action}/{apiId}", // "Help/{action}/{apiId}", TODO: Cambio URL autenticaci�n
                new { controller = "Help", action = "Index", apiId = UrlParameter.Optional });

            HelpPageConfig.Register(GlobalConfiguration.Configuration);
        }
    }
}
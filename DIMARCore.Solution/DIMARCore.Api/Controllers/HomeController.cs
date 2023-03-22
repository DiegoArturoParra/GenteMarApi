using System.Configuration;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace DIMARCore.Api.Controllers
{
    /// <summary>
    /// home controller
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Version index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            string versionAPI = ConfigurationManager.AppSettings["VersionGenteMarAPI"];

            ViewBag.Title = $"GENTE DE MAR {versionAPI} - API";

            return View();
        }
    }
}

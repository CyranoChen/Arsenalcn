using System.Reflection;
using System.Web.Mvc;
using Arsenal.Mobile.Models;

namespace Arsenal.Mobile.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [OutputCache(Duration = 3600)]
        public ActionResult _AssemblyPartial()
        {
            var model = new AssemblyDto(Assembly.GetExecutingAssembly());

            return PartialView(model);
        }
    }
}
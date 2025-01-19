using Microsoft.AspNetCore.Mvc;

namespace Sanctuary.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

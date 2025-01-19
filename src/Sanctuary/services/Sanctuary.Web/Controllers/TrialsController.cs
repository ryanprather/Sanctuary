using Microsoft.AspNetCore.Mvc;

namespace Sanctuary.Web.Controllers
{
    public class TrialsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

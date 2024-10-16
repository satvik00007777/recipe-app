using Microsoft.AspNetCore.Mvc;

namespace FinalProjectMVC.Controllers
{
    public class PreferenceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace FinalProjectMVC.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

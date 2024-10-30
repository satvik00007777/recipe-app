using Microsoft.AspNetCore.Mvc;

namespace FinalProjectMVC.Controllers
{
    public class CustomRecipeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace FinalProjectMVC.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        //[HttpPost]
        //public Task<IActionResult> Login()
        //{

        //}

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }
    }
}

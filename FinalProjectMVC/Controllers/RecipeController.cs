﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjectMVC.Controllers
{
    public class RecipeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
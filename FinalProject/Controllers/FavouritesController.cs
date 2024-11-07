using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavoritesController : ControllerBase
    {
        private static List<FavoriteRecipeDto> _favorites = new List<FavoriteRecipeDto>();

        [HttpGet]
        public IActionResult GetFavorites()
        {
            return Ok(_favorites);
        }

        //public Task<IActionResult> AddToFavorites(string uri)
        //{
        //    //string UniqueId = uri;
        //    //return View(UniqueId);
        //    return Ok(");
        //}
    }

    public class FavoriteRecipeDto
    {
        public string UniqueId { get; set; }
        public string Title { get; set; }
        public string SourceUrl { get; set; }
        public string ImageUrl { get; set; }
    }
}

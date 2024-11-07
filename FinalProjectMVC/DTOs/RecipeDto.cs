using System.Security.Policy;

namespace FinalProjectMVC.DTOs
{
    public class RecipeDto
    {
        public string UniqueId { get; set; }

        public string title { get; set; }

        public string ingredients { get; set; }

        public string instructions { get; set; }

        public string imageUrl { get; set; }

        public string source { get; set; }

        public string uri { get; set; }

    }
}

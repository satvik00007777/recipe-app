namespace FinalProject.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for recipe information.
    /// </summary>
    public class RecipeDto
    {
        public int RecipeId { get; set; }

        public string UniqueId { get; set; }

        public string Title { get; set; }

        public string Ingredients { get; set; }

        public string Instructions { get; set; }

        public string ImageUrl { get; set; }

        public string Source { get; set; }
    }
}

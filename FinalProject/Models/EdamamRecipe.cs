namespace FinalProject.Models
{
    /// <summary>
    /// Represents the response from the Edamam API for recipe search results.
    /// </summary>
    public class EdamamApiResponse
    {
        /// <summary>
        /// An array of hits, each containing a recipe object.
        /// </summary>
        public Hit[] hits { get; set; }
    }

    /// <summary>
    /// Represents a single hit in the Edamam API response, containing a recipe.
    /// </summary>
    public class Hit
    {
        /// <summary>
        /// The recipe data for the specific hit.
        /// </summary>
        public EdamamRecipe recipe { get; set; }
    }

    /// <summary>
    /// Represents detailed recipe information from the Edamam API.
    /// </summary>
    public class EdamamRecipe
    {
        public string label { get; set; }

        public Ingredient[] ingredients { get; set; }

        public string url { get; set; }

        public string image { get; set; }

        public string source { get; set; }

        public string uri { get; set; }
    }

    /// <summary>
    /// Represents an individual ingredient in a recipe from the Edamam API.
    /// </summary>
    public class Ingredient
    {
        public string text { get; set; }
    }
}

namespace FinalProject.Models
{
    public class EdamamApiResponse
    {
        public Hit[] hits { get; set; }
    }

    public class Hit
    {
        public EdamamRecipe recipe { get; set; }
    }

    public class EdamamRecipe
    {
        public string label { get; set; }

        public Ingredient[] ingredients { get; set; }

        public string url { get; set; }

        public string image { get; set; }

        public string source { get; set; }

        public string uri { get; set; }
    }

    public class Ingredient
    {
        public string text { get; set; }
    }
}

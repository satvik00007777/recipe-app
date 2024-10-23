using Microsoft.Identity.Client;

namespace FinalProject.Models;

public class EdamamApiResponse
{
    public Hit[] hits { get; set; }
}

public class Hit
{
    public Recipe recipe { get; set; }
}

public partial class Recipe
{
    public int RecipeId { get; set; }

    public string Title { get; set; } = null!;

    public string Ingredients { get; set; } = null!;

    public string Instructions { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public string? Source { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}

public class Ingredient
{
    public string text { get; set; }
}

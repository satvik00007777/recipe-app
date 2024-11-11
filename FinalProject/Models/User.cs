namespace FinalProject.Models;

/// <summary>
/// Represents a user entity with relevant details such as username, password, email, and associated recipes.
/// </summary>
public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Preferences { get; set; }

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}

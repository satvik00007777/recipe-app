namespace FinalProject.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for user signup information.
    /// </summary>
    public class SignupDto
    {
        public string Name { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Preferences { get; set; }
    }
}

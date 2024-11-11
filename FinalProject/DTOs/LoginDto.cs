namespace FinalProject.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for user login information.
    /// </summary>
    public class LoginDto
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}

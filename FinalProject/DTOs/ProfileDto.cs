namespace FinalProject.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for user profile information.
    /// </summary>
    public class ProfileDto
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }
    }
}

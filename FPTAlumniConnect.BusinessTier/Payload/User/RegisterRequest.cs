using System.ComponentModel.DataAnnotations;

namespace FPTAlumniConnect.BusinessTier.Payload.User
{
    public class RegisterRequest
    {
        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; } = null!;

        public string? ProfilePicture { get; set; }
        public int RoleId { get; set; } = 2;

        public int? MajorId { get; set; } 
    }
}

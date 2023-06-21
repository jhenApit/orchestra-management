using System.ComponentModel.DataAnnotations;

namespace OrchestraAPI.Dtos.User
{
    public class UserUpdateDto
    {
        [Required(ErrorMessage = "The User 'Username' is required")]
        [MaxLength(50, ErrorMessage = "Username cannot be longer than 50 characters")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "The User 'Password' is required")]
        [MaxLength(50, ErrorMessage = "Password cannot be longer than 50 characters")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "The 'Email' field is required")]
        [MaxLength(50, ErrorMessage = "Email cannot be longer than 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@gmail\.com$", ErrorMessage = "Email must be a valid Gmail address")]
        public string? Email { get; set; }
        public int Role { get; set; }
    }
}

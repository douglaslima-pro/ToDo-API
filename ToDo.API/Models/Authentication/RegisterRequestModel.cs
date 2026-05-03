using System.ComponentModel.DataAnnotations;

namespace ToDo.API.Models.Authentication
{
    public class RegisterRequestModel
    {
        [Required(ErrorMessage = "{0} is required")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [EmailAddress(ErrorMessage = "{0} is not a valid email address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [MinLength(6, ErrorMessage = "{0} must be at least {1} characters long")]
        [MaxLength(30, ErrorMessage = "{0} must be at most {1} characters long")]
        public string Password { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [MaxLength(100, ErrorMessage = "{0} must be at most {1} characters long")]
        public string? FirstName { get; set; }

        [MaxLength(100, ErrorMessage = "{0} must be at most {1} characters long")]
        public string? LastName { get; set; }

        [MaxLength(600, ErrorMessage = "{0} must be at most {1} characters long")]
        public string? Bio { get; set; }
    }
}

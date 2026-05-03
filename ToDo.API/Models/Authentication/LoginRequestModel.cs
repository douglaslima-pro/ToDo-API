using System.ComponentModel.DataAnnotations;

namespace ToDo.API.Models.Authentication
{
    public class LoginRequestModel
    {
        [Required(ErrorMessage = "{0} is required")]
        [EmailAddress(ErrorMessage = "{0} is not a valid email address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [MinLength(6, ErrorMessage = "{0} must be at least {1} characters long")]
        [MaxLength(30, ErrorMessage = "{0} must be at most {1} characters long")]
        public string? Password { get; set; }
    }
}

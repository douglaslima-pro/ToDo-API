using System.ComponentModel.DataAnnotations;

namespace ToDo.API.Models.Authentication
{
    public class GenerateEmailConfirmationTokenModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
        [EmailAddress(ErrorMessage = "{0} is not a valid email address")]
        public string? Email { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ToDo.API.Models.Authentication
{
    public class SendPasswordResetTokenModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
        [EmailAddress(ErrorMessage = "{0} is not a valid email address")]
        public string? Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
        public string? Name { get; set; }
    }
}

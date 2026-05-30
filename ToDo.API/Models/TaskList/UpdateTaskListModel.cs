using System.ComponentModel.DataAnnotations;

namespace ToDo.API.Models.TaskList
{
    public class UpdateTaskListModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
        [MinLength(5, ErrorMessage = "{0} must be at least {1} characters long")]
        [MaxLength(100, ErrorMessage = "{0} must be at most {1} characters long")]
        public string? Title { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ToDo.API.Models.TaskListItem
{
    public class UpdateTaskListItemModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
        [MinLength(5, ErrorMessage = "{0} must be at least {1} characters long")]
        [MaxLength(100, ErrorMessage = "{0} must be at most {1} characters long")]
        public string? Title { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
        [MinLength(5, ErrorMessage = "{0} must be at least {1} characters long")]
        [MaxLength(500, ErrorMessage = "{0} must be at most {1} characters long")]
        public string? Description { get; set; }
        
        [Required(ErrorMessage = "{0} is required")]
        public DateTime DueDate { get; set; }
    }
}

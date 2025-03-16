using System.ComponentModel.DataAnnotations;
using TaskManagementAPI.Models.Entities;

namespace TaskManagementAPI.Models.Dtos.Requests
{
    public class UpdateUserTaskRequest
    {

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public bool IsComplete { get; set; }
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public List<string> Tags { get; set; } = new();

    }
}

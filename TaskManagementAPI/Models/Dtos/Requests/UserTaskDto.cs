using TaskManagementAPI.Models.Entities;

namespace TaskManagementAPI.Models.Dtos.Requests
{
    public class UserTaskDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsComplete { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<string> Tags { get; set; } = new();

    }
}

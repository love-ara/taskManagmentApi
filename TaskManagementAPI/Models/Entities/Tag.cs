using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Models.Entities
{
    public class Tag
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        
        [Required]
        public string Name { get; set; }

        public List<UserTask> Tasks { get; set; } = new();
    }

}


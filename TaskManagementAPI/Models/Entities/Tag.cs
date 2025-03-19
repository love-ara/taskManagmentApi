using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Models.Entities
{
    public class Tag
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public virtual ICollection<UserTask> UserTasks { get; set; } = new List<UserTask>();
    }

}


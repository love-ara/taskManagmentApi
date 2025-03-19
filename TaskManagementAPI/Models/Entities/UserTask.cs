using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementAPI.Models.Entities
{
    public class UserTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public  string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;
        public bool IsComplete { get; set; } = false;
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Guid AppUserId { get; set; }

        [ForeignKey("AppUserId")]
        public virtual AppUser AppUser { get; set; }

        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();


        public UserTask()
        {

        }
      
    }
}

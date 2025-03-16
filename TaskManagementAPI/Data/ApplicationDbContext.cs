using TaskManagementAPI.Models.Entities;

using Microsoft.EntityFrameworkCore;

namespace TaskManagementAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
       
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserTask> UserTasks { get; set; }
        public DbSet<Tag> Tags { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserTask>().HasKey(t => t.Id);
            modelBuilder.Entity<Tag>().HasKey(t => t.Id);


            modelBuilder.Entity<UserTask>()
               .HasMany(t => t.Tags)
               .WithMany(t => t.Tasks);
        }

    }
}

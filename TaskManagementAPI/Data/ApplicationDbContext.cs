using TaskManagementAPI.Models.Entities;

using Microsoft.EntityFrameworkCore;

namespace TaskManagementAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
       
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<UserTask> UserTasks { get; set; }
        public DbSet<Tag> Tags { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserTask>().HasKey(t => t.Id);
            modelBuilder.Entity<Tag>().HasKey(t => t.Id);

            modelBuilder.Entity<AppUser>()
               .HasMany(u => u.UserTasks)
               .WithOne(t => t.AppUser)
               .HasForeignKey(t => t.AppUserId)
               .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<UserTask>()
               .HasMany(t => t.Tags)
               .WithMany(t => t.UserTasks)
               .UsingEntity(j => j.ToTable("UserTaskTags"));


            modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

        }

    }
}

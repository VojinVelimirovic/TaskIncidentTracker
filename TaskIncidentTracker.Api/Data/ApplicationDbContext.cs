using Microsoft.EntityFrameworkCore;
using System;
using TaskIncidentTracker.Api.Models;

namespace TaskIncidentTracker.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<TaskItem> Tasks { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();

            modelBuilder.Entity<TaskItem>()
        .HasOne(t => t.CreatedBy)
        .WithMany()
        .HasForeignKey(t => t.CreatedById)
        .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<TaskItem>()
                .HasMany(t => t.AssignedTo)
                .WithMany(u => u.Tasks)
                .UsingEntity<Dictionary<string, object>>(
                    "TaskItemUser",
                    j => j.HasOne<User>().WithMany().HasForeignKey("AssignedToId").OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne<TaskItem>().WithMany().HasForeignKey("TasksId").OnDelete(DeleteBehavior.Cascade)
                );
        }
    }
}

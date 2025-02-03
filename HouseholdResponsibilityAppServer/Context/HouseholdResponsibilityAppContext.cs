using Microsoft.EntityFrameworkCore;
using HouseholdResponsibilityAppServer.Models.Task;

namespace HouseholdResponsibilityAppServer.Context
{
    public class HouseholdResponsibilityAppContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<HouseholdTask> Tasks { get; set; }
        public DbSet<ScheduledTask> ScheduledTasks { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<HouseholdGroup> Groups { get; set; }
        public DbSet<Household> Households { get; set; }
        public DbSet<Review> Reviews { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HouseholdTask>()
                .HasKey(t => t.TaskId); // Primary Key

            modelBuilder.Entity<HouseholdTask>()
                .Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(255); // VARCHAR(255)

            modelBuilder.Entity<HouseholdTask>()
                .Property(t => t.CreatedAt)
                .HasDefaultValueSql("NOW()"); // Default value in DB

            modelBuilder.Entity<HouseholdTask>()
                .Property(t => t.Priority)
                .HasDefaultValue(false); // Default Priority = false

            modelBuilder.Entity<HouseholdTask>()
                .HasOne(t => t.CreatedBy) // Foreign Key relationship
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.CreatedBy);

            modelBuilder.Entity<HouseholdTask>()
                .HasOne(t => t.Group) // Foreign key to Group
                .WithMany(g => g.Tasks)
                .HasForeignKey(t => t.Group);
        }


    }

}

using Microsoft.EntityFrameworkCore;
using HouseholdResponsibilityAppServer.Models.Task;
using HouseholdResponsibilityAppServer.Models.ScheduledTasks;

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

            //HouseholdTask
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



            //ScheduledTask
            modelBuilder.Entity<ScheduledTask>(entity =>
            {
                // Primary Key
                entity.HasKey(st => st.ScheduledTaskId);

                // Foreign Keys
                entity.HasOne(st => st.HouseholdTask)
                    .WithMany()
                    .HasForeignKey(st => st.ScheduledTaskId)
                    .OnDelete(DeleteBehavior.Cascade); // If task is deleted, delete scheduled task

                entity.HasOne(st => st.CreatedBy)
                    .WithMany()
                    .HasForeignKey(st => st.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict); // Prevent deletion if referenced user exists

                entity.HasOne(st => st.AssignedTo)
                    .WithMany()
                    .HasForeignKey(st => st.AssignedTo)
                    .OnDelete(DeleteBehavior.SetNull); // If assigned user is deleted, set to NULL

                // Default Values
                entity.Property(st => st.CreatedAt)
                    .HasDefaultValueSql("NOW()");

                entity.Property(st => st.Daily)
                    .HasDefaultValue(false);

                entity.Property(st => st.Weekly)
                    .HasDefaultValue(false);

                entity.Property(st => st.Monthly)
                    .HasDefaultValue(false);

                entity.Property(st => st.NoRepeat)
                    .HasDefaultValue(false);

                // Ensure NoRepeat Requires EventDate
                entity.HasCheckConstraint("CHK_NoRepeat_EventDate", "no_repeat = FALSE OR event_date IS NOT NULL");

                // Optional Fields
                entity.Property(st => st.DayOfWeek)
                    .IsRequired(false);

                entity.Property(st => st.DayOfMonth)
                    .IsRequired(false);

                entity.Property(st => st.SpecificTime)
                    .IsRequired(false);
            });






        }


    }

}

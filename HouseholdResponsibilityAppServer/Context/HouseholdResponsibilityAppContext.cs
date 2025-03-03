using Microsoft.EntityFrameworkCore;
using HouseholdResponsibilityAppServer.Models.Task;
using HouseholdResponsibilityAppServer.Models.ScheduledTasks;
using HouseholdResponsibilityAppServer.Models.Histories;
using HouseholdResponsibilityAppServer.Models.Households;
using HouseholdResponsibilityAppServer.Models.Invitations;
using HouseholdResponsibilityAppServer.Models.Users;
using HouseholdResponsibilityAppServer.Models.Groups;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace HouseholdResponsibilityAppServer.Context
{
    public class HouseholdResponsibilityAppContext: IdentityDbContext<User, IdentityRole, string>
    {
        public HouseholdResponsibilityAppContext(DbContextOptions<HouseholdResponsibilityAppContext> options) 
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<HouseholdTask> Tasks { get; set; }
        public DbSet<ScheduledTask> ScheduledTasks { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<TaskGroup> Groups { get; set; }
        public DbSet<Household> Households { get; set; }
        //public DbSet<Review> Reviews { get; set; }
        public DbSet<Invitation> Invitations { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // HouseholdTask
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
                .WithMany();

            modelBuilder.Entity<HouseholdTask>()
                .HasOne(t => t.Group) // Foreign key to Group
                .WithMany();

            modelBuilder.Entity<HouseholdTask>()
                .HasOne(t => t.Household)
                .WithMany(h => h.HouseholdTasks);

            // ScheduledTask
            modelBuilder.Entity<ScheduledTask>(entity =>
            {
                // Primary Key
                entity.HasKey(st => st.ScheduledTaskId);

                // Foreign Key: HouseholdTask (One ScheduledTask belongs to one HouseholdTask)
                entity.HasOne(st => st.HouseholdTask)
                    .WithMany() // Assuming HouseholdTask does not have a ScheduledTask collection
                    .OnDelete(DeleteBehavior.Cascade); // If HouseholdTask is deleted, delete the ScheduledTask

                // Foreign Key: CreatedBy (User who created the task)
                entity.HasOne(st => st.CreatedBy)
                    .WithMany() // Assuming User does not have a ScheduledTask collection
                    .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of User if they created tasks

                // Foreign Key: AssignedTo (User assigned to the task)
                entity.HasOne(st => st.AssignedTo)
                    .WithMany() // Assuming User does not have a ScheduledTask collection
                    .OnDelete(DeleteBehavior.SetNull); // If assigned user is deleted, set AssignedTo to NULL

                entity.Property(st => st.Repeat)
                    .HasConversion<string>() // Stores enum as string (e.g., "Daily", "Weekly")
                    .IsRequired();

                // EventDate (Required)
                entity.Property(st => st.EventDate)
                    .IsRequired();

                // CreatedAt (Set default value to current time)
                entity.Property(st => st.CreatedAt)
                    .HasDefaultValueSql("NOW()")
                    .IsRequired();

                // AtSpecificTime (Boolean flag)
                entity.Property(st => st.AtSpecificTime)
                    .HasDefaultValue(false)
                    .IsRequired();
            });

            modelBuilder.Entity<User>()
                .HasOne(u => u.Household)
                .WithMany(h => h.Users)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            modelBuilder.Entity<Household>()
                .HasOne(h => h.CreatedByUser)
                .WithMany()
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of User if they created a Household

            modelBuilder.Entity<TaskGroup>()
                .HasOne(tg => tg.Household)
                .WithMany(h => h.Groups)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<History>()
                .HasOne(h => h.ScheduledTask) // Foreign Key relationship
                .WithMany();

            modelBuilder.Entity<History>()
                .HasOne(h => h.CompletedBy) // Foreign key to Group
                .WithMany();

            modelBuilder.Entity<History>()
                .HasOne(h => h.Household)
                .WithMany(h => h.Histories)
                .OnDelete(DeleteBehavior.Cascade);
        }


    }

}

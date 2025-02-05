using HouseholdResponsibilityAppServer.Models;
using Microsoft.EntityFrameworkCore;


namespace HouseholdResponsibilityAppServer.Data
{
    public class HouseholdResponsibilityContext : DbContext
    {
        public HouseholdResponsibilityContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Household> Households { get; set; }

        public DbSet<TaskGroup> Groups { get; set; }

        public DbSet<Invitation> Invitations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Household és User kapcsolata
            modelBuilder.Entity<User>()
                .HasOne(u => u.Household)
                .WithMany(h => h.Users)
                .HasForeignKey(u => u.HouseholdId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // CreatedBy kapcsolat kezelése
            modelBuilder.Entity<Household>()
                .HasOne(h => h.CreatedByUser)
                .WithMany()
                .HasForeignKey(h => h.CreatedBy)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }



    }
}

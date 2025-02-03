using Microsoft.EntityFrameworkCore;

namespace HouseholdResponsibilityAppServer.Context
{
    public class HouseholdResponsibilityAppContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<ScheduledTask> ScheduledTasks { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Household> Households { get; set; }
        public DbSet<Review> Reviews { get; set; }

    }
}

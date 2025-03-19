using HouseholdResponsibilityAppServer;
using HouseholdResponsibilityAppServer.Context;
using HouseholdResponsibilityAppServer.Models.Groups;
using HouseholdResponsibilityAppServer.Models.Histories;
using HouseholdResponsibilityAppServer.Models.Households;
using HouseholdResponsibilityAppServer.Models.ScheduledTasks;
using HouseholdResponsibilityAppServer.Models.Task;
using HouseholdResponsibilityAppServer.Models.Users;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.Common;

namespace IntegrationTests
{
    public class HRAWebAppFactory : WebApplicationFactory<Program>
    {
        private readonly string _dbName = Guid.NewGuid().ToString();
        private readonly string _userWithoutHouseholdId = "1";
        private readonly string _userWithHouseholdId = "2";
        public HouseholdResponsibilityAppContext InMemoryDbContext { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(async services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(
                     d => d.ServiceType ==
                    typeof(IDbContextOptionsConfiguration<HouseholdResponsibilityAppContext>));

                services.Remove(dbContextDescriptor);

                var dbConnectionDescriptor = services.SingleOrDefault(
                   d => d.ServiceType ==
                typeof(DbConnection));

                services.Remove(dbConnectionDescriptor);

                //Add new DbContextOptions for our two contexts, this time with inmemory db 
                services.AddDbContext<HouseholdResponsibilityAppContext>(options =>
                {
                    options.UseInMemoryDatabase(_dbName);
                });

                //We will need to initialize our in memory databases. 
                //Since DbContexts are scoped services, we create a scope
                using var scope = services.BuildServiceProvider().CreateScope();

                //We use this scope to request the registered dbcontexts, and initialize the schemas
                var householdContext = scope.ServiceProvider.GetRequiredService<HouseholdResponsibilityAppContext>();
                householdContext.Database.EnsureDeleted();
                householdContext.Database.EnsureCreated();

                InMemoryDbContext = householdContext;

                await SeedInMemoryDb(scope);
                //await AddHouseholdToInMemoryDb(scope);

            });
        }

        private async Task SeedInMemoryDb(IServiceScope scope)
        {
            using var householdContext = scope.ServiceProvider.GetRequiredService<HouseholdResponsibilityAppContext>();
            using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            // seed users to the in memory db.

            var userWithNoHousehold = new User
            {
                Id = _userWithoutHouseholdId,
                Email = "userWithNoHousehold@gmail.com",
                NormalizedEmail = "userWithNoHousehold@gmail.com",
                UserName = "userWithNoHousehold",
                FirstName = "John",
                LastName = "Doe",
                Household = null,
                PasswordHash = "password"
            };


            var userWithHousehold = new User
            {
                Id = _userWithHouseholdId,
                Email = "userWithHousehold@gmail.com",
                NormalizedEmail = "userWithHousehold@gmail.com",
                UserName = "userWithHousehold",
                FirstName = "John",
                LastName = "Doe",
                Household = null,
                PasswordHash = "password"
            };



            //add  a household to the user
            var household = new Household()
            {
                HouseholdId = 1,
                Name = "Household",
                CreatedByUser = userWithHousehold,
                CreatedAt = DateTime.UtcNow,
                Groups = new List<TaskGroup>(),
                Histories = new List<History>(),
                HouseholdTasks = new List<HouseholdTask>(),
                Users = new List<User>(),
            };

            userWithHousehold.Household = household;


            await userManager.CreateAsync(userWithNoHousehold, "password");
            await userManager.CreateAsync(userWithHousehold, "password");


            //for some reason this cannot be before we add users to the db (why?)
            var taskGroup = new TaskGroup()
            {
                GroupId = 1,
                Name = "Pre Seeded Group",
                Household = household,
            };
           

            await householdContext.Groups.AddAsync(taskGroup);
            userWithHousehold.Household.Groups.Add(taskGroup);


            var householdTask = new HouseholdTask()
            {
                TaskId = 1,
                CreatedAt = DateTime.Now,
                CreatedBy = userWithHousehold,
                Title = "test title",
                Description = "test description",
                Group = taskGroup,
                Household = household,
                Priority = false,
            };

            var scheduledTask = new ScheduledTask()
            {
                ScheduledTaskId = 1,
                AssignedTo = userWithHousehold,
                AtSpecificTime = false,
                CreatedAt = DateTime.Now,
                CreatedBy = userWithHousehold,
                EventDate = DateTime.UtcNow,
                Repeat = 0,
                HouseholdTask = householdTask,
            };

            var history = new History()
            {
                HistoryId = 1,
                CompletedAt = DateTime.UtcNow,
                CompletedBy = userWithHousehold,
                ScheduledTask = scheduledTask,
                Household = household,
                Outcome = false,
            };



            await householdContext.Tasks.AddAsync(householdTask);
            await householdContext.ScheduledTasks.AddAsync(scheduledTask);
            await householdContext.Histories.AddAsync(history);


            await householdContext.SaveChangesAsync();


        }

    }
}
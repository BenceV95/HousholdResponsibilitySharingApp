using HouseholdResponsibilityAppServer;
using HouseholdResponsibilityAppServer.Context;
using HouseholdResponsibilityAppServer.Models.Households;
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

                await AddUsersToInMemoryDb(scope);



            });
        }


        private async Task AddUsersToInMemoryDb(IServiceScope scope)
        {
            var householdContext = scope.ServiceProvider.GetRequiredService<HouseholdResponsibilityAppContext>();
            using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            // seed users to the in memory db.

            var userWithNoHousehold = new User
            {
                Email = "userWithNoHousehold@gmail.com",
                UserName = "userWithNoHousehold",
                FirstName = "John",
                LastName = "Doe",
                Household = null,
                PasswordHash = "password"
            };


            var userWithHousehold = new User
            {
                Email = "userWithHousehold@gmail.com",
                NormalizedEmail = "userWithHousehold@gmail.com",
                UserName = "userWithHousehold",
                FirstName = "John",
                LastName = "Doe",
                Household = null,
                PasswordHash = "password"
            };

            //add  a household to the user
            userWithHousehold.Household = new Household()
            {
                Name = "Household",
                CreatedByUser = userWithHousehold,
                CreatedAt = DateTime.UtcNow,
                Groups = null,
                Histories = null,
                HouseholdId = 1,
                HouseholdTasks = null,
                Users = null,
            };


            var resultUserWithNoHousehold = await userManager.CreateAsync(userWithNoHousehold, "password");
            var resultUserWithHousehold = await userManager.CreateAsync(userWithHousehold, "password");

            await householdContext.SaveChangesAsync();

        }
    }
}
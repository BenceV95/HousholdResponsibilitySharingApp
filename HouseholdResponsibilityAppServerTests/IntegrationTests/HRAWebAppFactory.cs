using HouseholdResponsibilityAppServer;
using HouseholdResponsibilityAppServer.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
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


                //var dbContextDescriptor2 = services.SingleOrDefault(
                //     d => d.ServiceType ==
                //    typeof(IDbContextOptionsConfiguration<HouseholdResponsibilityAppContext>));
                //    services.Remove(dbContextDescriptor2);

                var dbContextDescriptor = services.SingleOrDefault(
                     d => d.ServiceType ==
                    typeof(IDbContextOptionsConfiguration<HouseholdResponsibilityAppContext>));

                     services.Remove(dbContextDescriptor);

                        var dbConnectionDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbConnection));

                     services.Remove(dbConnectionDescriptor);





                //var dbContextDescriptor = services.SingleOrDefault(
                //    d => d.ServiceType == typeof(DbContextOptions<HouseholdResponsibilityAppContext>));

                //if (dbContextDescriptor != null)
                //{
                //    services.Remove(dbContextDescriptor);
                //}



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


                //Here we could do more initializing if we wished (e.g. adding admin user)


            });
        }
    }
}
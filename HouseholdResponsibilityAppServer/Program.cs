
using HouseholdResponsibilityAppServer.Context;
using HouseholdResponsibilityAppServer.Repositories.Groups;
using HouseholdResponsibilityAppServer.Repositories.HouseholdRepo;
using HouseholdResponsibilityAppServer.Repositories.InvitationRepo;
using HouseholdResponsibilityAppServer.Services.Groups;
using HouseholdResponsibilityAppServer.Services.HouseholdService;
using HouseholdResponsibilityAppServer.Services.Invitation;
using HouseholdResponsibilityAppServer.Services.UserService;
using HouseholdResponsibilityAppServer.Repositories.UserRepo;
using Microsoft.EntityFrameworkCore;
using HouseholdResponsibilityAppServer.Services.HouseholdTaskServices;
using HouseholdResponsibilityAppServer.Repositories.HouseholdTasks;
using HouseholdResponsibilityAppServer.Repositories.Histories;
using HouseholdResponsibilityAppServer.Services.HistoryServices;
using HouseholdResponsibilityAppServer.Repositories.ScheduledTasks;
using HouseholdResponsibilityAppServer.Services.ScheduledTaskServices;

namespace HouseholdResponsibilityAppServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("http://localhost:3000") // Allow the frontend domain
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<HouseholdResponsibilityAppContext>(options =>
            {
                options.UseNpgsql(
                    "Server=localhost;Port=5432;User Id=postgres;Password=postgres;Database=HouseholdResponsibility;");
            });


            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IHouseholdRepository, HouseholdRepository>();
            builder.Services.AddScoped<IGroupRepository, GroupRepository>();
            builder.Services.AddScoped<IInvitationRepository, InvitationRepository>();

            builder.Services.AddScoped<IHouseholdTaskService, HouseholdTaskService>();
            builder.Services.AddScoped<IHouseholdTasksRepository, HouseholdTasksRepository>();
            builder.Services.AddScoped<IHistoryRepository, HistoryRepository>();
            builder.Services.AddScoped<IHistoryService, HistoryService>();
            builder.Services.AddScoped<IScheduledTasksRepository, ScheduledTasksRepository>();
            builder.Services.AddScoped<IScheduledTaskService, ScheduledTaskService>();



            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IHouseholdService, HouseholdService>();
            builder.Services.AddScoped<IGroupService, GroupService>();
            builder.Services.AddScoped<IInvitationService, InvitationService>();





            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

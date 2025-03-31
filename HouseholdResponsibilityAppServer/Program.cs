
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using HouseholdResponsibilityAppServer.Services.Authentication;
using Microsoft.OpenApi.Models;
using HouseholdResponsibilityAppServer.Models.Users;

namespace HouseholdResponsibilityAppServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Household API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
            });
            builder.Services.AddDbContext<HouseholdResponsibilityAppContext>(options =>
            {
                //options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
                options.UseNpgsql(builder.Configuration["DbConnectionString"]); // user secrets used here
            });

            // Config settings
            var jwtSettings = builder.Configuration;
            var issuerSigningKey = jwtSettings["IssuerSigningKey"];


            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddScoped<IHouseholdRepository, HouseholdRepository>();
            builder.Services.AddScoped<IHouseholdService, HouseholdService>();

            builder.Services.AddScoped<IGroupRepository, GroupRepository>();
            builder.Services.AddScoped<IGroupService, GroupService>();

            builder.Services.AddScoped<IHouseholdTaskService, HouseholdTaskService>();
            builder.Services.AddScoped<IHouseholdTasksRepository, HouseholdTasksRepository>();

            builder.Services.AddScoped<IHistoryRepository, HistoryRepository>();
            builder.Services.AddScoped<IHistoryService, HistoryService>();

            builder.Services.AddScoped<IScheduledTasksRepository, ScheduledTasksRepository>();
            builder.Services.AddScoped<IScheduledTaskService, ScheduledTaskService>();

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<AuthenticationSeeder>();

            builder.Services.AddScoped<IInvitationRepository, InvitationRepository>();
            builder.Services.AddScoped<IInvitationService, InvitationService>();



            AddAuth();
            AddIdentity();


            var app = builder.Build();


            if (app.Environment.IsDevelopment())
            {

                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            using var scope = app.Services.CreateScope(); // AuthenticationSeeder is a scoped service, therefore we need a scope instance to access it
            var authenticationSeeder = scope.ServiceProvider.GetRequiredService<AuthenticationSeeder>();
            authenticationSeeder.AddRoles();
            authenticationSeeder.AddAdmin();

            /*
            // Apply pending migrations

            var db = scope.ServiceProvider.GetRequiredService<HouseholdResponsibilityAppContext>();
            db.Database.Migrate();
            */

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
            return;

            void AddAuth()
            {
                builder.Services
             .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters()
                 {
                     ClockSkew = TimeSpan.Zero,
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = jwtSettings["ValidIssuer"],
                     ValidAudience = jwtSettings["ValidAudience"],
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(issuerSigningKey)),
                 };


                 options.Events = new JwtBearerEvents
                 {
                     OnMessageReceived = context =>
                     {
                         if (string.IsNullOrEmpty(context.Token))
                         {
                             context.Token = context.Request.Cookies["token"];
                         }
                         return Task.CompletedTask;
                     }
                 };
             });
            }

            void AddIdentity()
            {
                builder.Services
                    .AddIdentityCore<User>(options =>
                    {
                        options.SignIn.RequireConfirmedAccount = false;
                        options.User.RequireUniqueEmail = true;
                        options.Password.RequireDigit = false;
                        options.Password.RequiredLength = 2; //for now, it needs to be only 2 digits long
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequireLowercase = false;
                    })
                    .AddRoles<IdentityRole>() //Enable Identity roles 
                    .AddEntityFrameworkStores<HouseholdResponsibilityAppContext>();
            }

        }
    }
}



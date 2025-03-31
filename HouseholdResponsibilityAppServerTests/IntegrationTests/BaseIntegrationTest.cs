using System.Net.Http.Json;
using HouseholdResponsibilityAppServer.Context;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests;

public partial class HouseholdTaskTests
{
    public abstract class BaseIntegrationTest
    {
        protected readonly HRAWebAppFactory _app;
        protected readonly HttpClient _client;
        protected readonly HouseholdResponsibilityAppContext _dbContext;
        protected readonly string _userWithHouseholdEmail = "userWithHousehold@gmail.com";
        protected readonly string _userWithoutHouseholdEmail = "userWithNoHousehold@gmail.com";
        protected readonly string _userPassword = "password";

        protected BaseIntegrationTest()
        {
            _app = new HRAWebAppFactory();
            _client = _app.CreateClient(new WebApplicationFactoryClientOptions
            {
                HandleCookies = true
            });

            var scope = _app.Services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<HouseholdResponsibilityAppContext>();
        }

        protected async Task<HttpResponseMessage> LoginUser(string email, string password)
        {
            var loginRequest = new
            {
                Email = email,
                Password = password
            };

            return await _client.PostAsJsonAsync("/Auth/Login", loginRequest);
        }

        protected void AttachAuthCookies(HttpResponseMessage loginResponse)
        {
            var cookies = loginResponse.Headers.GetValues("Set-Cookie").ToList();
            foreach (var cookie in cookies)
            {
                _client.DefaultRequestHeaders.Add("Cookie", cookie);
            }
        }
    }
}
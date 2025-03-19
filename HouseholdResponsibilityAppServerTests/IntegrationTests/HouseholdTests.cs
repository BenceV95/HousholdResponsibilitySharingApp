using HouseholdResponsibilityAppServer.Context;
using HouseholdResponsibilityAppServer.Models.Households;
using IntegrationTests;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests
{
    public class HouseholdTests
    {
        [Collection("IntegrationTests")]
        public class HouseholdControllerIntegrationTest
        {
            private readonly HRAWebAppFactory _app;
            private readonly HttpClient _client;
            private readonly HouseholdResponsibilityAppContext _dbContext;
            private readonly string _userWithHouseholdEmail = "userWithHousehold@gmail.com"; //pre-seeded user
            private readonly string _userWithoutHouseholdEmail = "userWithNoHousehold@gmail.com";
            private readonly string _userPassword = "password";


            public HouseholdControllerIntegrationTest()
            {


                _app = new HRAWebAppFactory();

                //automatic cookie handling
                _client = _app.CreateClient(new WebApplicationFactoryClientOptions
                {
                    HandleCookies = true
                });

                var scope = _app.Services.CreateScope();
                _dbContext = scope.ServiceProvider.GetRequiredService<HouseholdResponsibilityAppContext>();

            }

            private async Task<HttpResponseMessage> LoginUser(string email, string password)
            {
                var loginRequest = new
                {
                    Email = email,
                    Password = password
                };

                return await _client.PostAsJsonAsync("/Auth/Login", loginRequest);
            }

            private void AttachAuthCookies(HttpResponseMessage loginResponse)
            {
                var cookies = loginResponse.Headers.GetValues("Set-Cookie").ToList();
                foreach (var cookie in cookies)
                {
                    _client.DefaultRequestHeaders.Add("Cookie", cookie);
                }
            }



            [Fact]
            public async Task GetAllHouseholds_ShouldReturnOk_WithHouseholdList()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                var response = await _client.GetAsync("/households");

                var responseContent = await response.Content.ReadAsStringAsync();
                var households = JsonConvert.DeserializeObject<List<HouseholdResponseDto>>(responseContent);

                Assert.NotEmpty(households);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }

            // dont know what kind of exception to make here so its commented out for now

            //[Fact]
            //public async Task GetAllHouseholds_ShouldReturnInternalServerError_WhenExceptionOccurs()
            //{
            //    var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
            //    loginResponse.EnsureSuccessStatusCode();
            //    AttachAuthCookies(loginResponse);


            //    var response = await _client.GetAsync("/households");

            //    Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

            //    var responseContent = await response.Content.ReadAsStringAsync();
            //    var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

            //    Assert.Equal("An error occurred while retrieving all Households.", errorResponse["Message"]);
            //}




            [Fact]
            public async Task GetHouseholdById_ShouldReturnOk_WhenHouseholdExists()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                const int existingHouseholdId = 1;

                var response = await _client.GetAsync($"/household/{existingHouseholdId}");

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var household = await response.Content.ReadFromJsonAsync<HouseholdResponseDto>();

                Assert.NotNull(household);
                Assert.Equal(existingHouseholdId, household.HouseholdResponseDtoId);
            }


            [Fact]
            public async Task GetHouseholdById_ShouldReturnNotFound_WhenHouseholdDoesNotExist()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                const int nonExistingHouseholdId = 9999;

                var response = await _client.GetAsync($"/household/{nonExistingHouseholdId}");

                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

                var responseContent = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

                Assert.Equal(errorResponse["message"], $"Household with ID {nonExistingHouseholdId} not found.");
            }



            [Fact]
            public async Task CreateHousehold_ShouldReturnOk_WhenValidDataIsProvided()
            {
                var loginResponse = await LoginUser(_userWithoutHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                var createRequest = new HouseholdDto
                {
                    HouseholdName = "new household",
                 
                };

                var postResponse = await _client.PostAsJsonAsync("/household", createRequest);

                Assert.Equal(HttpStatusCode.OK, postResponse.StatusCode);

                var responseContent = await postResponse.Content.ReadAsStringAsync();
                var newHouseholdId = JsonConvert.DeserializeObject<int>(responseContent);

                Assert.True(newHouseholdId > 0);
            }



            [Fact]
            public async Task CreateHousehold_ShouldReturnBadRequest_WhenUserAlreadyHaveAHousehold()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                var createRequest = new HouseholdDto
                {
                    HouseholdName = "new household",

                };

                var postResponse = await _client.PostAsJsonAsync("/household", createRequest);

                Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);

                var responseContent = await postResponse.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

                Assert.Equal(errorResponse["message"], $"User already has a household.");
            }



            [Fact]
            public async Task DeleteHousehold_ShouldReturnNoContent_WhenHouseholdExists()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                const int householdId = 1;

                var deleteResponse = await _client.DeleteAsync($"/household/{householdId}");

                Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
            }


            [Fact]
            public async Task DeleteHousehold_ShouldReturnNotFound_WhenHouseholdDoesNotExist()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                const int nonExistingHouseholdId = 999;

                var deleteResponse = await _client.DeleteAsync($"/household/{nonExistingHouseholdId}");

                Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
            }



            [Fact]
            public async Task UpdateHousehold_ShouldReturnNoContent_WhenUpdateIsSuccessful()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                const int householdId = 1;

                var updateRequest = new HouseholdDto
                {
                    HouseholdName = "new name",

                };

                var putResponse = await _client.PutAsJsonAsync($"/household/{householdId}", updateRequest);

                Assert.Equal(HttpStatusCode.NoContent, putResponse.StatusCode);
            }



            [Fact]
            public async Task UpdateHousehold_ShouldReturnNotFound_WhenHouseholdDoesNotExist()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                const int nonExistingHouseholdId = 999;

                var updateRequest = new HouseholdDto
                {
                    HouseholdName = "new name",

                };

                var putResponse = await _client.PutAsJsonAsync($"/household/{nonExistingHouseholdId}", updateRequest);

                Assert.Equal(HttpStatusCode.NotFound, putResponse.StatusCode);
            }



        }
    }
}
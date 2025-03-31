using HouseholdResponsibilityAppServer.Context;
using HouseholdResponsibilityAppServer.Models.ScheduledTasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests
{
    public class ScheduledTaskTests
    {
        [Collection("IntegrationTests")]
        public class ScheduledTaskControllerIntegrationTest
        {
            private readonly HRAWebAppFactory _app;
            private readonly HttpClient _client;
            private readonly HouseholdResponsibilityAppContext _dbContext;
            private readonly string _userWithHouseholdEmail = "userWithHousehold@gmail.com"; //pre-seeded user
            private readonly string _userWithoutHouseholdEmail = "userWithNoHousehold@gmail.com";
            private readonly string _userPassword = "password";


            public ScheduledTaskControllerIntegrationTest()
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
            public async Task GetAllTasks_ShouldReturnOk_WhenTasksExist()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                var response = await _client.GetAsync("/scheduleds");
                var responseContent = await response.Content.ReadAsStringAsync();
                var scheduleds = JsonConvert.DeserializeObject<List<ScheduledTaskDTO>>(responseContent);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.NotEmpty(scheduleds);
            }





            //[Fact]
            //public async Task GetAllTasks_ShouldReturnInternalServerError_WhenServiceFails()
            //{
            //    var response = await _client.GetAsync("/scheduled");

            //    Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

            //    var content = await response.Content.ReadAsStringAsync();
            //    Assert.Contains("An error occurred while retrieving all Scheduled Tasks.", content);
            //}



            [Fact]
            public async Task GetTaskById_ShouldReturnOk_WhenTaskExists()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                const int existingScheduledId = 1;



                var response = await _client.GetAsync($"/scheduled/{existingScheduledId}");

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }




            [Fact]
            public async Task GetTaskById_ShouldReturnInternalServerError_WhenTaskDoesNotExist()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                const int nonExistingScheduledId = 999;


                var response = await _client.GetAsync($"/scheduled/{nonExistingScheduledId}");

                Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            }




            [Fact]
            public async Task PostNewTask_ShouldReturnOk_WhenTaskIsCreated()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                var validRequest = new CreateScheduledTaskRequest()
                {
                    EventDate = DateTime.UtcNow,
                    AssignedToUserId = "2",
                    AtSpecificTime = false,
                    HouseholdTaskId = 1,
                    Repeat = 0,
                };

                var response = await _client.PostAsJsonAsync("/scheduled", validRequest);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var taskId = await response.Content.ReadAsStringAsync();
                Assert.NotNull(taskId);
            }



            [Fact]
            public async Task PostNewTask_ShouldReturnBadRequest_WhenRequestIsInvalid()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                var invalidRequest = new CreateScheduledTaskRequest() //missing properties
                {
                    AtSpecificTime = false,
                    EventDate = DateTime.UtcNow,
                    Repeat = 0
                };

                var response = await _client.PostAsJsonAsync("/scheduled", invalidRequest);

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }






            [Fact]
            public async Task UpdateTask_ShouldReturnOk_WhenTaskIsUpdated()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                const int validScheduledTaskId = 1;

                var validRequest = new CreateScheduledTaskRequest()
                {
                    AssignedToUserId = "2",
                    AtSpecificTime = true,
                    EventDate = DateTime.UtcNow,
                    HouseholdTaskId = 1,
                    Repeat = 0
                };


                var updateResponse = await _client.PatchAsJsonAsync($"/scheduled/{validScheduledTaskId}", validRequest);

                Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
            }




            [Fact]
            public async Task UpdateTask_ShouldReturnInternalServerError_WhenTaskDoesNotExist()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                const int invalidScheduledTaskId = 999;

                var validRequest = new CreateScheduledTaskRequest()
                {
                    AssignedToUserId = "2",
                    AtSpecificTime = true,
                    EventDate = DateTime.UtcNow,
                    HouseholdTaskId = 1,
                    Repeat = 0
                };

                var updateResponse = await _client.PatchAsJsonAsync($"/scheduled/{invalidScheduledTaskId}", validRequest);

                Assert.Equal(HttpStatusCode.InternalServerError, updateResponse.StatusCode);
            }





            [Fact]
            public async Task DeleteScheduledTask_ShouldReturnNoContent_WhenTaskIsDeleted()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                const int existingScheduledId = 1;

                var deleteResponse = await _client.DeleteAsync($"/scheduled/{existingScheduledId}");


                Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
            }



            [Fact]
            public async Task DeleteScheduledTask_ShouldReturnInternalServerError_WhenTaskDoesNotExist()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                const int nonExistingScheduledId = 999;

                var response = await _client.DeleteAsync($"/scheduled/{nonExistingScheduledId}");

                Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
                var responseContent = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

                //could be more specific 
                Assert.Equal(errorResponse["message"], $"An error occurred while deleting Scheduled Task.");
            }





            [Fact]
            public async Task GetAllScheduledsByHousehold_ShouldReturnOk_WhenTasksExist()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);


                var response = await _client.GetAsync("/scheduleds/my-household");
                var responseContent = await response.Content.ReadAsStringAsync();
                var scheduleds = JsonConvert.DeserializeObject<List<ScheduledTaskDTO>>(responseContent);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.NotEmpty(scheduleds);

            }



            [Fact]
            public async Task GetAllScheduledsByHousehold_ShouldReturnInternalServerError_WhenUserIsNotInAHousehold()
            {
                var loginResponse = await LoginUser(_userWithoutHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                var response = await _client.GetAsync("/scheduleds/my-household");

                Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            }

        }
    }
}
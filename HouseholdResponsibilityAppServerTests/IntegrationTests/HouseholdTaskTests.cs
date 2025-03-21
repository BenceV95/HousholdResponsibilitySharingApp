using HouseholdResponsibilityAppServer.Models.HouseholdTasks;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests
{
    public partial class HouseholdTaskTests
    {
        [Collection("IntegrationTests")]
        public class HouseholdTaskControllerIntegrationTest : BaseIntegrationTest
        {
            /*
            private readonly HRAWebAppFactory _app;
            private readonly HttpClient _client;
            private readonly HouseholdResponsibilityAppContext _dbContext;
            private readonly string _userWithHouseholdEmail = "userWithHousehold@gmail.com"; //pre-seeded user
            private readonly string _userWithoutHouseholdEmail = "userWithNoHousehold@gmail.com";
            private readonly string _userPassword = "password";


            public HouseholdTaskControllerIntegrationTest()
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

            // extension method for httpclient or base class, inheritance
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
            */

            [Fact]
            public async Task GetHouseholdTasksByUsersHousehold_ShouldReturnOk_WithListOfTasks()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                var response = await _client.GetAsync("/tasks/my-household");

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var tasks = JsonConvert.DeserializeObject<List<HouseholdTaskDTO>>(await response.Content.ReadAsStringAsync());
                Assert.NotEmpty(tasks);
            }


            // not sure what kind of error to throw

            //[Fact]
            //public async Task GetHouseholdTasksByUsersHousehold_ShouldReturnInternalServerError_WhenServiceFails()
            //{
            //    var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
            //    loginResponse.EnsureSuccessStatusCode();
            //    AttachAuthCookies(loginResponse);


            //    var response = await _client.GetAsync("/tasks/my-household");

            //    Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            //}



            [Fact]
            public async Task GetAllTasks_ShouldReturnOk_WithListOfTasks()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                var response = await _client.GetAsync("/tasks");

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var tasks = JsonConvert.DeserializeObject<List<HouseholdTaskDTO>>(await response.Content.ReadAsStringAsync());
                Assert.NotEmpty(tasks);
            }



            //[Fact]
            //public async Task GetAllTasks_ShouldReturnInternalServerError_WhenServiceFails()
            //{
            //    var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
            //    loginResponse.EnsureSuccessStatusCode();
            //    AttachAuthCookies(loginResponse);



            //    var response = await _client.GetAsync("/tasks");

            //    Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            //}




            [Fact]
            public async Task GetTaskById_ShouldReturnOk_WhenTaskExists()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                const int existingTaskId = 1;
                var response = await _client.GetAsync($"/task/{existingTaskId}");

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }



            [Fact]
            public async Task GetTaskById_ShouldReturnInternalServerError_WhenTaskDoesNotExist()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                const int nonExistingId = 999;

                var response = await _client.GetAsync($"/task/{nonExistingId}");

                Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

                var responseContent = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

                Assert.Equal(errorResponse["message"], $"An error occurred while retrieving tasks with ID: {nonExistingId}.");
            }







            [Fact]
            public async Task PostNewTask_ShouldReturnOk_WhenDataIsValid()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                var createRequest = new CreateHouseholdTaskRequest { Title = "New Task", Description = "Test Task", GroupId = 1, Priority = false };
                var response = await _client.PostAsJsonAsync("/task", createRequest);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }



            [Fact]
            public async Task PostNewTask_ShouldReturnBadRequest_WhenDataIsInvalid()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                var createRequest = new CreateHouseholdTaskRequest { Title = "" }; // insufficient data
                var response = await _client.PostAsJsonAsync("/task", createRequest);

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }



            [Fact]
            public async Task UpdateTask_ShouldReturnOk_WhenDataIsValid()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                var updateRequest = new CreateHouseholdTaskRequest { Title = "Updated Task", Description = "Updated Description", Priority = true, GroupId = 1 };
                var response = await _client.PatchAsJsonAsync("/task/1", updateRequest);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }




            [Fact]
            public async Task UpdateTask_ShouldReturnBadRequest_WhenDataIsInvalid()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                var updateRequest = new CreateHouseholdTaskRequest { Title = "" }; // Invalid data
                var response = await _client.PatchAsJsonAsync("/task/1", updateRequest);

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }




            [Fact]
            public async Task DeleteTask_ShouldReturnNoContent_WhenDeletingExistingTask()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                const int existingTaskId = 1;
                var deleteResponse = await _client.DeleteAsync($"/task/{existingTaskId}");

                Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
            }


             //maybe should return not found?

            [Fact]
            public async Task DeleteTask_ShouldReturnInternalServerError_WhenDeletingNonExistingTask()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                const int nonExistingTaskId = 9999;

                var deleteResponse = await _client.DeleteAsync($"/task/{nonExistingTaskId}");

                Assert.Equal(HttpStatusCode.InternalServerError, deleteResponse.StatusCode);
            }


        }
    }
}
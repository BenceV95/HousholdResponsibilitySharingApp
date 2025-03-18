using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Net;
using HouseholdResponsibilityAppServer.Models.Histories;
using HouseholdResponsibilityAppServer.Context;
using Microsoft.Extensions.DependencyInjection;
using HouseholdResponsibilityAppServer.Models.Households;
using HouseholdResponsibilityAppServer.Models.ScheduledTasks;

namespace IntegrationTests
{
    public class HistoryTests
    {


        [Collection("IntegrationTests")]
        public class HistoryControllerIntegrationTest
        {
            private readonly HRAWebAppFactory _app;
            private readonly HttpClient _client;
            private readonly HouseholdResponsibilityAppContext _dbContext;
            private readonly string _userWithHouseholdEmail = "userWithHousehold@gmail.com"; //pre-seeded user
            private readonly string _userWithoutHouseholdEmail = "userWithNoHousehold@gmail.com";
            private readonly string _userPassword = "password";


            public HistoryControllerIntegrationTest()
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


            //get history by id


            [Fact]
            public async Task PostNewHistory_ShouldReturnBadRequest_WhenUserIdIsNotInDb()
            {


                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                const string notValidId = "not valid id";

                var createRequest = new CreateHistoryRequest
                {
                    CompletedByUserId = notValidId,
                    CompletedAt = DateTime.UtcNow,
                    HouseholdId = 1,
                    Outcome = true,
                    ScheduledTaskId = 1,

                };


                var postHistoryResponse = await _client.PostAsJsonAsync("/history", createRequest);

                Assert.Equal(HttpStatusCode.BadRequest, postHistoryResponse.StatusCode);
                var responseContent = await postHistoryResponse.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

                Assert.Equal(errorResponse["message"], $"User with ID {notValidId} not found.");
            }

            [Fact]
            public async Task PostNewHistory_ShouldReturnBadRequest_WhenScheduledTaskIsNotInDb()
            {


                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);


                var createRequest = new CreateHistoryRequest
                {
                    CompletedByUserId = "2",
                    CompletedAt = DateTime.UtcNow,
                    HouseholdId = 1,
                    Outcome = true,
                    ScheduledTaskId = 99, //non existing

                };


                var postHistoryResponse = await _client.PostAsJsonAsync("/history", createRequest);

                Assert.Equal(HttpStatusCode.BadRequest, postHistoryResponse.StatusCode);
                var responseContent = await postHistoryResponse.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

                Assert.Equal(errorResponse["message"], "No scheduled task was found with given Id!");
            }

            [Fact]
            public async Task PostNewHistory_ShouldReturnOk_WhenEveryDataIsValid()
            {


                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);


                var createRequest = new CreateHistoryRequest
                {
                    CompletedByUserId = "2",
                    CompletedAt = DateTime.UtcNow,
                    HouseholdId = 1,
                    Outcome = true,
                    ScheduledTaskId = 1,

                };


                var postHistoryResponse = await _client.PostAsJsonAsync("/history", createRequest);

                Assert.Equal(HttpStatusCode.OK, postHistoryResponse.StatusCode);
                var responseContent = await postHistoryResponse.Content.ReadAsStringAsync();
                var newHistoryId = JsonConvert.DeserializeObject<Int32>(responseContent);

                Assert.Equal(2, newHistoryId); // 1 history is seeded into db, so new id should be 2
            }






            [Fact]
            public async Task GetAllHistories_ShouldReturnOk_WithSeededHistory()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                var response = await _client.GetAsync("/histories");

                var responseContent = await response.Content.ReadAsStringAsync();


                var histories = JsonConvert.DeserializeObject<List<HistoryDTO>>(responseContent);

                Assert.NotEmpty(histories);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }


            //atm this fails, but should be fixed in the controller, or there should be a filter to give back only histories that belong to a specific household
            [Fact]
            public async Task GetAllHistories_ShouldReturnOkWithEmptyList_WhenUserDoesntHaveAHousehold()
            {
                var loginResponse = await LoginUser(_userWithoutHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                var response = await _client.GetAsync("/histories");

                var responseContent = await response.Content.ReadAsStringAsync();


                var histories = JsonConvert.DeserializeObject<List<HistoryDTO>>(responseContent);

                Assert.Empty(histories);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }

            [Fact]
            public async Task GetHistoryById_ShouldReturnOk_WhenHistoryExists()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                const int existingHistoryId = 1;

                var response = await _client.GetAsync($"/history/{existingHistoryId}");
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }

            [Fact]
            public async Task GetHistoryById_ShouldReturnBadRequest_WhenHistoryDoesNotExist()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                var response = await _client.GetAsync("/history/9999");

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }


            // this also fails, cause there is no id in the create history request, and in the querey params we dont use it further!
            [Fact]
            public async Task UpdateHistory_ShouldReturnOkAndUpdateEntry_WhenEveryDataIsValid()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                const int existingHistoryId = 1;


                var updateRequest = new CreateHistoryRequest
                {
                    CompletedByUserId = "2",
                    CompletedAt = DateTime.UtcNow,
                    HouseholdId = 1,
                    Outcome = true,
                    ScheduledTaskId = 1
                };

                var updateResponse = await _client.PatchAsJsonAsync($"/history/{existingHistoryId}", updateRequest);
                Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

                var updatedHistory = await _dbContext.Histories.FindAsync(existingHistoryId);

                Assert.NotNull(updatedHistory);
                Assert.True(updatedHistory.Outcome); //previously it was false
            }

            [Fact]
            public async Task DeleteHistory_ShouldReturnNoContent_WhenDeletingExistingHistory()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                const int existingHistoryId = 1;


                var deleteResponse = await _client.DeleteAsync($"/history/{existingHistoryId}");
                Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

                var deletedHistory = await _dbContext.Histories.FindAsync(existingHistoryId);
                Assert.Null(deletedHistory);
            }


            //should fix this as well! (test is good, the repo, or service is at fault)
            [Fact]
            public async Task DeleteHistory_ShouldReturnBadRequest_WhenDeletingNonExistingHistory()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                const int nonExistingHistoryId = 999;


                var deleteResponse = await _client.DeleteAsync($"/history/{nonExistingHistoryId}");
                Assert.Equal(HttpStatusCode.BadRequest, deleteResponse.StatusCode);

            }

            [Fact]
            public async Task GetHistoryById_ReturnsOk_WithValidId()
            {

                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                var historyId = 1;

                var response = await _client.GetAsync($"/history/{historyId}");

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var result = await response.Content.ReadFromJsonAsync<object>();

                Assert.NotNull(result);
            }

            [Fact]
            public async Task GetHistoryById_ReturnsBadRequest_WhenIdNotFound()
            {

                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);


                var historyId = 99;

                var response = await _client.GetAsync($"/history/{historyId}");

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

                var responseContent = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

                Assert.Equal(errorResponse["message"], "No history entry was found with given Id!");

            }

        }
    }
}
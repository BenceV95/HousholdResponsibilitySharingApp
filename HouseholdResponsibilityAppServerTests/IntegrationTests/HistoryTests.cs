using HouseholdResponsibilityAppServer.Contracts;
using IntegrationTests.ResponseModels;
using IntegrationTests;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HouseholdResponsibilityAppServer.Models.Histories;
using HouseholdResponsibilityAppServer.Models.ScheduledTasks;
using HouseholdResponsibilityAppServer.Models.HouseholdTasks;
using HouseholdResponsibilityAppServer.Models.Households;
using HouseholdResponsibilityAppServer.Models.Groups;
using HouseholdResponsibilityAppServer.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using HouseholdResponsibilityAppServer.Models.Users;

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


            //get histories
            //get history by id
            //post history
            //patch /modify history
            //delete history


            [Fact]
            public async Task PostNewHistory_ShouldReturnBadRequest_WhenScheduledTaskDoesntExist()
            {


                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);


                var createRequest = new CreateHistoryRequest
                {
                    CompletedByUserId = "userId",
                    CompletedAt = DateTime.UtcNow,
                    HouseholdId = 1,
                    Outcome = true,
                    ScheduledTaskId = 1,

                };


                var postHistoryResponse = await _client.PostAsJsonAsync("/history", createRequest);

                Assert.Equal(HttpStatusCode.BadRequest, postHistoryResponse.StatusCode);
                var responseContent = await postHistoryResponse.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

                Assert.Equal(errorResponse["message"], "No scheduled task was found with given Id!");
            }

            [Fact]
            public async Task GetAllHistories_ShouldReturnOk()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                var response = await _client.GetAsync("/history");

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }

            //[Fact]
            //public async Task GetHistoryById_ShouldReturnOk_WhenHistoryExists()
            //{
            //    var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
            //    loginResponse.EnsureSuccessStatusCode();
            //    AttachAuthCookies(loginResponse);

            //    var history = new History
            //    {
            //        CompletedBy = user,
            //        CompletedAt = DateTime.UtcNow,
            //        HouseholdId = 1,
            //        Outcome = true,
            //        ScheduledTaskId = 1
            //    };

            //    _dbContext.Histories.Add(history);
            //    await _dbContext.SaveChangesAsync();

            //    var response = await _client.GetAsync($"/history/{history.HistoryId}");
            //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            //}

            [Fact]
            public async Task GetHistoryById_ShouldReturnBadRequest_WhenHistoryDoesNotExist()
            {
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();
                AttachAuthCookies(loginResponse);

                var response = await _client.GetAsync("/history/9999");

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }

            //[Fact]
            //public async Task PostNewHistory_ShouldReturnOkAndSaveHistory()
            //{
            //    var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
            //    loginResponse.EnsureSuccessStatusCode();
            //    AttachAuthCookies(loginResponse);

            //    var createRequest = new CreateHistoryRequest
            //    {
            //        CompletedByUserId = "2",
            //        CompletedAt = DateTime.UtcNow,
            //        HouseholdId = 1,
            //        Outcome = true,
            //        ScheduledTaskId = 1
            //    };

            //    var postResponse = await _client.PostAsJsonAsync("/history", createRequest);
            //    Assert.Equal(HttpStatusCode.OK, postResponse.StatusCode);

            //    var historyId = await postResponse.Content.ReadAsAsync<int>();
            //    var savedHistory = await _dbContext.Histories.FindAsync(historyId);

            //    Assert.NotNull(savedHistory);
            //    Assert.Equal(createRequest.CompletedByUserId, savedHistory.CompletedByUserId);
            //    Assert.Equal(createRequest.HouseholdId, savedHistory.HouseholdId);
            //}

            //[Fact]
            //public async Task UpdateHistory_ShouldReturnOkAndUpdateEntry()
            //{
            //    var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
            //    loginResponse.EnsureSuccessStatusCode();
            //    AttachAuthCookies(loginResponse);

            //    var history = new History
            //    {
            //        CompletedByUserId = "2",
            //        CompletedAt = DateTime.UtcNow,
            //        HouseholdId = 1,
            //        Outcome = false,
            //        ScheduledTaskId = 1
            //    };

            //    _dbContext.Histories.Add(history);
            //    await _dbContext.SaveChangesAsync();

            //    var updateRequest = new CreateHistoryRequest
            //    {
            //        CompletedByUserId = "2",
            //        CompletedAt = DateTime.UtcNow,
            //        HouseholdId = 1,
            //        Outcome = true,
            //        ScheduledTaskId = 1
            //    };

            //    var updateResponse = await _client.PatchAsJsonAsync($"/history/{history.HistoryId}", updateRequest);
            //    Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

            //    var updatedHistory = await _dbContext.Histories.FindAsync(history.HistoryId);
            //    Assert.NotNull(updatedHistory);
            //    Assert.True(updatedHistory.Outcome);
            //}

            //[Fact]
            //public async Task DeleteHistory_ShouldReturnNoContent()
            //{
            //    var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
            //    loginResponse.EnsureSuccessStatusCode();
            //    AttachAuthCookies(loginResponse);

            //    var history = new History
            //    {
            //        CompletedByUserId = "2",
            //        CompletedAt = DateTime.UtcNow,
            //        HouseholdId = 1,
            //        Outcome = true,
            //        ScheduledTaskId = 1
            //    };

            //    _dbContext.Histories.Add(history);
            //    await _dbContext.SaveChangesAsync();

            //    var deleteResponse = await _client.DeleteAsync($"/history/{history.HistoryId}");
            //    Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            //    var deletedHistory = await _dbContext.Histories.FindAsync(history.HistoryId);
            //    Assert.Null(deletedHistory);
            //}
            //[Fact]
            //public async Task PostNewHistory_ShouldReturnOk_WhenGivenValidData()
            //{

            //    var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);

            //    loginResponse.EnsureSuccessStatusCode();

            //    AttachAuthCookies(loginResponse);



            //    var createTaskResponse = await _client.PostAsJsonAsync("/task", new CreateHouseholdTaskRequest()
            //    {
            //        Title = "Test",
            //        Description = "Test",
            //        GroupId = groupId,
            //        Priority = false,
            //    });

            //    createTaskResponse.EnsureSuccessStatusCode();







            //    var postScheduledResponse = await _client.PostAsJsonAsync("/scheduled", new CreateScheduledTaskRequest()
            //    {
            //        EventDate = DateTime.UtcNow,
            //        AssignedToUserId = "userId",
            //        AtSpecificTime = false,
            //        HouseholdTaskId = 1,
            //        Repeat = 0
            //    },
            //        taskId);





            //    var createRequest = new CreateHistoryRequest
            //    {
            //        CompletedByUserId = "userId",
            //        CompletedAt = DateTime.UtcNow,
            //        HouseholdId = 1,
            //        Outcome = true,
            //        ScheduledTaskId = 1,

            //    };


            //    var postHistoryResponse = await _client.PostAsJsonAsync("/history", createRequest);

            //    Assert.Equal(HttpStatusCode.BadRequest, postHistoryResponse.StatusCode);
            //    var responseContent = await postHistoryResponse.Content.ReadAsStringAsync();
            //    var succesMessage = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

            //    Assert.Equal(succesMessage["message"], "should be all good");
            //}




        }

    }
}


//[HttpGet()]
//public async Task<IActionResult> GetAllHistories()
//{
//    try
//    {
//        var histories = await _historyService.GetallHistoriesAsync();
//        return Ok(histories);
//    }
//    catch (Exception ex)
//    {
//        _logger.LogError(ex.Message);

//        return StatusCode(500, new { Message = "An error occurred while retrieving all histories." });
//    }
//}

//[HttpGet("/history/{historyId}")]
//public async Task<IActionResult> GetHistoryById(int historyId)
//{
//    try
//    {
//        var history = await _historyService.GetByIdAsync(historyId);
//        return Ok(history);
//    }
//    catch (KeyNotFoundException e)
//    {
//        return BadRequest(new { Message = e.Message });
//    }
//    catch (Exception ex)
//    {
//        _logger.LogError(ex.Message);

//        return StatusCode(500, new { Message = "An error occurred while retrieving history." });
//    }
//}

//[HttpPost("/history")]
//public async Task<IActionResult> PostNewHistory([FromBody] CreateHistoryRequest createRequest)
//{
//    try
//    {
//        var history = await _historyService.AddHistoryAsync(createRequest);
//        return Ok(history.HistoryId);
//    }
//    catch (KeyNotFoundException e)
//    {
//        return BadRequest(new { Message = e.Message });
//    }
//    catch (Exception ex)
//    {
//        _logger.LogError(ex.Message);

//        return StatusCode(500, new { Message = "An error occurred while posting history." });
//    }

//}


//[HttpPatch("/history/{historyId}")]
//public async Task<IActionResult> UpdateHistory([FromBody] CreateHistoryRequest updateRequest)
//{
//    try
//    {
//        var history = await _historyService.UpdateHistoryAsync(updateRequest);
//        return Ok(history);
//    }
//    catch (KeyNotFoundException e)
//    {
//        return BadRequest(new { Message = e.Message });
//    }
//    catch (Exception ex)
//    {
//        _logger.LogError(ex.Message);

//        return StatusCode(500, new { Message = "An error occurred while updating history." });
//    }
//}

//[HttpDelete("/history/{historyId}")]
//public async Task<IActionResult> DeleteHistory([FromQuery] int id)
//{
//    try
//    {
//        await _historyService.DeleteHistoryByIdAsync(id);
//        return NoContent();
//    }
//    catch (KeyNotFoundException e)
//    {
//        return BadRequest(new { Message = e.Message });
//    }
//    catch (Exception ex)
//    {
//        _logger.LogError(ex.Message);

//        return StatusCode(500, new { Message = "An error occurred while deleting history." });
//    }
//}
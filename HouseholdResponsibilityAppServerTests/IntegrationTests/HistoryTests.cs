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

namespace HouseholdResponsibilityAppServerTests.IntegrationTests
{
    public class HistoryTests
    {


        [Collection("IntegrationTests")]
        public class HistoryControllerIntegrationTest
        {
            private readonly HRAWebAppFactory _app;
            private readonly HttpClient _client;
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

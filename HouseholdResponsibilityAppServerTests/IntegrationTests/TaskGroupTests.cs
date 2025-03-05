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
using HouseholdResponsibilityAppServer.Models.Groups;
using HouseholdResponsibilityAppServer.Models.Households;

namespace IntegrationTests
{
    public class TaskGroupTests
    {
        [Collection("IntegrationTests")]
        public class TaskGroupControllerIntegrationTest
        {
            private readonly HRAWebAppFactory _app;
            private readonly HttpClient _client;
            private readonly string _userWithHouseholdEmail = "userWithHousehold@gmail.com"; //pre-seeded user
            private readonly string _userWithoutHouseholdEmail = "userWithNoHousehold@gmail.com";
            private readonly string _userPassword = "password";
            //private readonly HttpResponseMessage _loginResponse; maybe save this, so we dont have to login always


            public TaskGroupControllerIntegrationTest()
            {


                _app = new HRAWebAppFactory();

                _client = _app.CreateClient(new WebApplicationFactoryClientOptions
                {
                    HandleCookies = true
                });

            }


            /// <summary>
            /// Logs in a user with given email and password.
            /// </summary>
            /// <param name="email"></param>
            /// <param name="password"></param>
            /// <returns></returns>
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
            public async Task CreateGroup_ShouldReturnBadRequest_WhenUserIsNotPartOfAHousehold()
            {

                var loginResponse = await LoginUser(_userWithoutHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();


                AttachAuthCookies(loginResponse);

                var postGroupDto = new PostGroupDto
                {
                    GroupName = "groupName"
                };

                var response = await _client.PostAsJsonAsync("/group", postGroupDto);
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

                var result = await response.Content.ReadFromJsonAsync<object>();
                Assert.Contains("Cannot create group, user is not in a household!", result.ToString());
            }


            [Fact]
            public async Task CreateGroup_ShouldReturnOk_WhenUserIsPartOfAHousehold()
            {

                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);

                loginResponse.EnsureSuccessStatusCode();

                AttachAuthCookies(loginResponse);

                var postGroupDto = new PostGroupDto
                {
                    GroupName = "groupName"
                };

                var response = await _client.PostAsJsonAsync("/group", postGroupDto);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var responseContent = await response.Content.ReadAsStringAsync();
                var successResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

                Assert.Equal(successResponse["message"], "Group created successfully");
            }




            [Fact]
            public async Task GetGroupsByHouseholdId_ShouldReturnOk_WhenUserIsPartOfAHousehold()
            {


                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();

                AttachAuthCookies(loginResponse);

                //post a new task group
                var postGroupDto = new PostGroupDto
                {
                    GroupName = "groupName"
                };

                // create  a new task group
                var postGroupResponse = await _client.PostAsJsonAsync("/group", postGroupDto);
                Assert.Equal(HttpStatusCode.OK, postGroupResponse.StatusCode);


                //call the endpoint to get the groups associated with the user's household
                var getGroupsResponse = await _client.GetAsync("/groups/my-household");

                getGroupsResponse.EnsureSuccessStatusCode();

                var groups = await getGroupsResponse.Content.ReadFromJsonAsync<IEnumerable<GroupDto>>();


                Assert.Contains(groups, group => group.Name == "groupName");
            }

            [Fact]
            public async Task GetGroupsByHouseholdID_ShouldReturnInternalServerError_WhenUserIsNotInHousehold()
            {


                //log in the user
                var loginResponse = await LoginUser(_userWithoutHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();

                AttachAuthCookies(loginResponse);


                var response = await _client.GetAsync("/groups/my-household");

                Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

                var responseContent = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

                Assert.Equal(errorResponse["message"], "An error occurred while retrieving groups.");
            }




        }
    }
}

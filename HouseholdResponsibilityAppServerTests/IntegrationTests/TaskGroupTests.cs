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


            public TaskGroupControllerIntegrationTest()
            {


                _app = new HRAWebAppFactory();

                _client = _app.CreateClient(new WebApplicationFactoryClientOptions
                {
                    HandleCookies = true
                });

            }

            private async Task<int> CreateHousehold(string householdName)
            {
                var householdDto = new HouseholdDto
                {
                    HouseholdName = householdName
                };

                var response = await _client.PostAsJsonAsync("/household", householdDto);


                response.EnsureSuccessStatusCode();
                var householdId = await response.Content.ReadFromJsonAsync<int>();


                return householdId;
            }

            private async Task<HttpResponseMessage> RefreshToken()
            {
                var response = await _client.GetAsync("/Auth/update-token");

                response.EnsureSuccessStatusCode();

                return response;

            }



            /// <summary>
            /// This method registers a user with given data.
            /// </summary>
            /// <returns></returns>
            private async Task<HttpResponseMessage> RegisterUser(string email, string username, string password, string firstName, string lastName)
            {
                var registerRequest = new
                {
                    Email = email,
                    Username = username,
                    Password = password,
                    FirstName = firstName,
                    LastName = lastName,
                };

                return await _client.PostAsJsonAsync("/Auth/Register", registerRequest);
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

                string email = "testUser1@gmail.com";
                string password = "password";
                string username = "username";
                string firstName = "John";
                string lastName = "Doe";

                var registerResponse = await RegisterUser(email, username, password, firstName, lastName);
                registerResponse.EnsureSuccessStatusCode();

                var loginResponse = await LoginUser(email, password);
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


            //[Fact]
            //public async Task CreateGroup_ShouldReturnOk_WhenUserIsPartOfAHousehold()
            //{

            //    string email = "userWithHousehold@gmail.com"; //pre-seeded user
            //    string password = "password";

            //    var loginResponse = await LoginUser(email, password);

            //    var responseContent = await loginResponse.Content.ReadAsStringAsync();
            //    var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(responseContent);
            //    loginResponse.EnsureSuccessStatusCode();


            //    AttachAuthCookies(loginResponse);

            //    var postGroupDto = new PostGroupDto
            //    {
            //        GroupName = "groupName"
            //    };

            //    var response = await _client.PostAsJsonAsync("/group", postGroupDto);
            //    Console.WriteLine(response.ToString());
            //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            //    var result = await response.Content.ReadFromJsonAsync<object>();
            //    Assert.Contains("Cannot create group, user is not in a household!", result.ToString());
            //}


            [Fact]
            public async Task CreateGroup_ShouldReturnOk_WhenUserIsPartOfAHousehold()
            {

                string email = "testUser1@gmail.com";
                string password = "password";
                string username = "username";
                string firstName = "John";
                string lastName = "Doe";

                // register the user
                var registerResponse = await RegisterUser(email, username, password, firstName, lastName);
                registerResponse.EnsureSuccessStatusCode();


                //log in the user
                var loginResponse = await LoginUser(email, password);
                loginResponse.EnsureSuccessStatusCode();


                loginResponse.EnsureSuccessStatusCode();

                AttachAuthCookies(loginResponse);

                // create the household
                var createHousehold = await CreateHousehold("household");

                //refresh the household id in the token
                var refreshResponse = await RefreshToken();

                AttachAuthCookies(refreshResponse);


                var postGroupDto = new PostGroupDto
                {
                    GroupName = "groupName"
                };
                // create  a new task group
                var response = await _client.PostAsJsonAsync("/group", postGroupDto);
                Console.WriteLine(response.ToString());
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var result = await response.Content.ReadFromJsonAsync<object>();
                Assert.Contains("Group created successfully", result.ToString());
            }




        }
    }
}

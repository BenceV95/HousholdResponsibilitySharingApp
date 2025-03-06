using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using HouseholdResponsibilityAppServer.Models.Groups;
using Microsoft.AspNetCore.Http;

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

            //CREATE GROUP

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



            //GET GROUPS BY HOUSEHOLD ID

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

                //should contain the pre seeded group as well
                Assert.Equal(2, groups.Count());

                Assert.Contains(groups, group => group.Name == "groupName");
                Assert.Contains(groups, group => group.Name == "Pre Seeded Group");
            }

            [Fact]
            public async Task GetGroupsByHouseholdID_ShouldReturnInternalServerError_WhenUserIsNotInHousehold()
            {

                var loginResponse = await LoginUser(_userWithoutHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();

                AttachAuthCookies(loginResponse);


                var response = await _client.GetAsync("/groups/my-household");

                Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

                var responseContent = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

                Assert.Equal(errorResponse["message"], "An error occurred while retrieving groups.");
            }


            // GET GROUP BY ID

            [Fact]
            public async Task GetGroupById_ShouldReturnOk_WhenGroupExists()
            {
                const int preSeededGroupId = 1;

                var loginResponse = await LoginUser(_userWithoutHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();

                AttachAuthCookies(loginResponse);

                var response = await _client.GetAsync($"/group/{preSeededGroupId}");
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var succesResponse = JsonConvert.DeserializeObject<GroupResponseDto>(responseContent);

                Assert.Equal(succesResponse.Name, "Pre Seeded Group");
                Assert.Equal(succesResponse.GroupResponseDtoId, 1);

                Assert.Equal(succesResponse.HouseholdId, 1);

            }


            [Fact]
            public async Task GetGroupById_ShouldReturnBadRequest_WhenGroupDoesntExist()
            {
                const int nonExistingGroupId = -1;

                var loginResponse = await LoginUser(_userWithoutHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();

                AttachAuthCookies(loginResponse);

                var response = await _client.GetAsync($"/group/{nonExistingGroupId}");
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

                var responseContent = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

                Assert.Equal(errorResponse["message"], $"Group with ID {nonExistingGroupId} not found.");


            }

            //GET GROUPS (not sure how to test this for bad outcome)

            [Fact]
            public async Task GetGroups_ShouldReturnOk_WhenUsedWithAuthorizedUser()
            {

                var loginResponse = await LoginUser(_userWithoutHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();

                AttachAuthCookies(loginResponse);

                var response = await _client.GetAsync($"/groups");
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var responseContent = await response.Content.ReadAsStringAsync();
                var succesResponse = JsonConvert.DeserializeObject<List<GroupResponseDto>>(responseContent);

                Assert.True(succesResponse.Count() == 1);
                Assert.Contains(succesResponse, group => group.Name == "Pre Seeded Group");


            }


            //UPDATE GROUP
            [Fact]
            public async Task UpdateGroup_ShouldReturnNoContent_WhenPassedValidData()
            {
                var groupDto = new GroupDto()
                {
                    Name = "Updated Group"
                };

                const int existingGroupId = 1;
                var loginResponse = await LoginUser(_userWithHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();

                AttachAuthCookies(loginResponse);

                // serialize groupDto to JSON
                var json = JsonConvert.SerializeObject(groupDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PutAsync($"/group/{existingGroupId}", content);

                //for now, it just returns no content
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                //var responseContent = await response.Content.ReadAsStringAsync();
                //var succesResponse = JsonConvert.DeserializeObject<List<GroupResponseDto>>(responseContent);
                //Assert.True(succesResponse.Count() == 1);
                //Assert.Contains(succesResponse, group => group.Name == "Pre Seeded Group");

            }


            [Fact]
            public async Task UpdateGroup_ShouldReturnBadRequest_WhenUserIsNotInAHousehold()
            {
                var groupDto = new GroupDto()
                {
                    Name = "Updated Group"
                };

                const int existingGroupId = 1;
                var loginResponse = await LoginUser(_userWithoutHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();

                AttachAuthCookies(loginResponse);

                // serialize groupDto to JSON
                var json = JsonConvert.SerializeObject(groupDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PutAsync($"/group/{existingGroupId}", content);

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

                var responseContent = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
                Assert.Equal(errorResponse["message"], "Cannot update group, user is not in a household!");


            }


            //DELETE GROUP

            [Fact]
            public async Task DeleteGroup_ShouldReturnNoContent_WhenPassedExistingGroupId()
            {

                //possible bug : user without household should be able to delete a group?

                const int existingGroupId = 1;
                var loginResponse = await LoginUser(_userWithoutHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();

                AttachAuthCookies(loginResponse);


                var response = await _client.DeleteAsync($"/group/{existingGroupId}");

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                //var responseContent = await response.Content.ReadAsStringAsync();
                //var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
                //Assert.Equal(errorResponse["message"], "Cannot update group, user is not in a household!");


            }

            [Fact]
            public async Task DeleteGroup_ShouldReturnInternalServerError_WhenPassedNonExistingGroupId()
            {

                //possible bug : user without household should be able to delete a group?

                const int nonExistingGroupId = -1;
                var loginResponse = await LoginUser(_userWithoutHouseholdEmail, _userPassword);
                loginResponse.EnsureSuccessStatusCode();

                AttachAuthCookies(loginResponse);


                var response = await _client.DeleteAsync($"/group/{nonExistingGroupId}");

                Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

                var responseContent = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
                Assert.Equal(errorResponse["message"], "An error occurred while deleting group.");


            }
        }
    }
}



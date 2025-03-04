using HouseholdResponsibilityAppServer.Contracts;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net;
using Newtonsoft.Json;
using IntegrationTests.ResponseModels;




namespace IntegrationTests
{
    public class AuthTests
    {

        [Collection("IntegrationTests")]
        public class MyControllerIntegrationTest
        {
            private readonly HRAWebAppFactory _app;
            private readonly HttpClient _client;


            public MyControllerIntegrationTest()
            {


                _app = new HRAWebAppFactory();

                //automatic cookie handling
                _client = _app.CreateClient(new WebApplicationFactoryClientOptions
                {
                    HandleCookies = true
                });

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


            [Fact]
            public async Task Register_ShouldReturnCreated_WhenGivenValidData()
            {
                // Act
                string email = "testUser1@gmail.com";
                string password = "password";
                string username = "username";
                string firstName = "firstName";
                string lastName = "lastName";


                var response = await RegisterUser(email, username, password, firstName, lastName);

                // Assert
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);

                var result = await response.Content.ReadFromJsonAsync<RegistrationResponse>();
                Assert.NotNull(result);
                Assert.Equal("testUser1@gmail.com", result.Email);
                Assert.Equal("username", result.UserName);
            }


            [Theory]
            [InlineData("", "testUser1", "password", "asddas", "user", "Email", "The Email field is required.")]
            [InlineData("testUser1@gmail.com", "", "password", "asddas", "user", "Username", "The Username field is required.")]
            [InlineData("testUser1@gmail.com", "testUser1", "password", "", "user", "FirstName", "The FirstName field is required.")]
            [InlineData("testUser1@gmail.com", "testUser1", "password", "asddas", "", "LastName", "The LastName field is required.")]
            public async Task Register_ShouldReturnBadRequestWithErrors_WhenFieldIsMissing(
                string email, string username, string password, string firstName, string lastName, string field, string expectedErrorMessage)
            {
                // Arrange

                // Act
                var response = await RegisterUser(email, username, password, firstName, lastName);

                // Assert
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                var responseContent = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<ValidationErrorResponse>(responseContent);
                Assert.Contains(errorResponse.Errors[field], errorMessage => errorMessage.Contains(expectedErrorMessage));
            }


            [Fact]
            public async Task Register_ShouldReturnBadRequest_WhenEmailAlreadyExists()
            {
                string duplicateEmail = "testUser1@gmail.com";
                string duplicateUsername = "testUser1";
                string password = "password";
                string firstName = "firstName";
                string lastName = "lastName";

                // Act
                var goodResponse = await RegisterUser(duplicateEmail, duplicateUsername, password, firstName, lastName);

                var badResponse = await RegisterUser(duplicateEmail, duplicateUsername, password, firstName, lastName); //duplicate values

                // Assert
                Assert.Equal(HttpStatusCode.BadRequest, badResponse.StatusCode);

                var responseContent = await badResponse.Content.ReadAsStringAsync();


                var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(responseContent); // for some reason I couldn't make it work with ValidationErrorResponse


                Assert.Contains(errorResponse["DuplicateEmail"], errorMessage => errorMessage.Contains("Email 'testUser1@gmail.com' is already taken."));
                Assert.Contains(errorResponse["DuplicateUserName"], errorMessage => errorMessage.Contains("Username 'testUser1' is already taken."));
            }



            [Fact]
            public async Task Login_ShouldReturnBadRequest_WhenModelIsInvalid()
            {
                string email = "";
                string password = "password";

                var response = await LoginUser(email, password);


                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

                var responseContent = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(responseContent);

                Assert.Contains(errorResponse["Bad credentials"], errorMessage => errorMessage.Contains("Invalid email"));
            }


            [Fact]
            public async Task Login_ShouldReturnBadRequest_WhenEmailDoesntExistInDb()
            {

                string email = "nonExisting@gmail.com";
                string password = "password";

                var response = await LoginUser(email, password);

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

                var responseContent = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(responseContent);

                Assert.Contains(errorResponse["Bad credentials"], errorMessage => errorMessage.Contains("Invalid email"));
            }


            [Fact]
            public async Task Login_ShouldReturnBadRequest_WhenPwIsWrong()
            {

                string email = "testUser1@gmail.com";
                string username = "testUser1";
                string password = "password";
                string firstName = "firstName";
                string lastName = "lastName";

                var registerResponse = await RegisterUser(email, username, password, firstName, lastName);


                registerResponse.EnsureSuccessStatusCode();


                var loginResponse = await LoginUser(email, "WRONGPASSWORD");


                Assert.Equal(HttpStatusCode.BadRequest, loginResponse.StatusCode);

                var responseContent = await loginResponse.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(responseContent);

                Assert.Contains(errorResponse["Bad credentials"], errorMessage => errorMessage.Contains("Invalid password"));
            }




            [Fact]
            public async Task Login_ShouldReturnOk_WhenGivenCorrectValues()
            {

                string email = "testUser1@gmail.com";
                string username = "testUser1";
                string password = "password";
                string firstName = "firstName";
                string lastName = "lastName";

                var response = await RegisterUser(email, username, password, firstName, lastName);


                response.EnsureSuccessStatusCode();


                //check if we can log in with the newly created acc

                var loginResponse = await LoginUser(email, password);
                loginResponse.EnsureSuccessStatusCode();

            }



        }



    }
}
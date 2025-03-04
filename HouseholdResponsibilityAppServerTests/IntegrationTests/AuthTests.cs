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



            [Fact]
            public async Task Register_ShouldReturnCreated_WhenGivenValidData()
            {
                // Arrange
                var request = new
                {
                    Email = "testUser1@gmail.com",
                    Username = "testUser1",
                    Password = "password",
                    FirstName = "test",
                    LastName = "user",
                };

                // Act
                var response = await _client.PostAsJsonAsync("/Auth/Register", request);

                // Assert
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);

                var result = await response.Content.ReadFromJsonAsync<RegistrationResponse>();
                Assert.NotNull(result);
                Assert.Equal("testUser1@gmail.com", result.Email);
                Assert.Equal("testUser1", result.UserName);
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
                var invalidRequest = new
                {
                    Email = email,
                    Username = username,
                    Password = password,
                    FirstName = firstName,
                    LastName = lastName,
                };

                // Act
                var response = await _client.PostAsJsonAsync("/Auth/Register", invalidRequest);

                // Assert
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                var responseContent = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<ValidationErrorResponse>(responseContent);
                Assert.Contains(errorResponse.Errors[field], errorMessage => errorMessage.Contains(expectedErrorMessage));
            }


            [Fact]
            public async Task Register_ShouldReturnBadRequest_WhenEmailAlreadyExists()
            {
                // Arrange
                var existingUser = new
                {
                    Email = "testUser1@gmail.com",
                    Username = "testUser1",
                    Password = "password",
                    FirstName = "test",
                    LastName = "user",
                };

                // Act
                var goodResponse = await _client.PostAsJsonAsync("/Auth/Register", existingUser);

                var badResponse = await _client.PostAsJsonAsync("/Auth/Register", existingUser); //duplicate values

                // Assert
                Assert.Equal(HttpStatusCode.BadRequest, badResponse.StatusCode);

                var responseContent = await badResponse.Content.ReadAsStringAsync();


                var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(responseContent); // for some reason I couldn't make it work with ValidationErrorResponse


                Assert.Contains(errorResponse["DuplicateEmail"], errorMessage => errorMessage.Contains("Email 'testUser1@gmail.com' is already taken."));
                Assert.Contains(errorResponse["DuplicateUserName"], errorMessage => errorMessage.Contains("Username 'testUser1' is already taken."));
            }


        }



    }
}
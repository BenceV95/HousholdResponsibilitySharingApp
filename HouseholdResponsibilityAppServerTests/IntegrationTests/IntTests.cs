using HouseholdResponsibilityAppServer.Contracts;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;




namespace IntegrationTests
{
    public class IntTests
    {

        [Collection("IntegrationTests")] //this is to avoid problems with tests running in parallel
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

            // ❌ Test 2: Invalid Data (400 Bad Request)
            [Fact]
            public async Task Register_ShouldReturnBadRequest_WhenDataIsInvalid()
            {
                // Arrange
                var invalidRequest = new
                {
                    Email = "invalid-email",
                    UserName = "",  // Missing username
                    Password = "pw"  // Too short if you have validation
                };

                // Act
                var response = await _client.PostAsJsonAsync("/Auth/Register", invalidRequest);

                // Assert
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

                var error = await response.Content.ReadAsStringAsync();
                //Assert.Contains("The Email field is not a valid e-mail address.", error);
            }

            // 🔁 Test 3: Duplicate Email (400 Bad Request)
            [Fact]
            public async Task Register_ShouldReturnBadRequest_WhenEmailAlreadyExists()
            {
                // Arrange
                var existingUser = new
                {
                    Email = "admin@gmail.com",  // Seeded user in the database
                    UserName = "admin",
                    Password = "password"
                };

                // Act
                var response = await _client.PostAsJsonAsync("/Auth/Register", existingUser);

                // Assert
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

                var error = await response.Content.ReadAsStringAsync();
                //Assert.Contains("Email already exists", error);  // Modify based on your error message
            }


        }



    }
}

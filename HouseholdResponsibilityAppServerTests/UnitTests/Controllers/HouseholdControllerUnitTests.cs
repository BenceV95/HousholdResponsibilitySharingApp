using HouseholdResponsibilityAppServer.Controllers;
using HouseholdResponsibilityAppServer.Models;
using HouseholdResponsibilityAppServer.Models.Households;
using HouseholdResponsibilityAppServer.Models.Users;
using HouseholdResponsibilityAppServer.Services.Authentication;
using HouseholdResponsibilityAppServer.Services.HouseholdService;
using HouseholdResponsibilityAppServer.Services.Invitation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace HouseholdResponsibilityAppServerTests.UnitTests.Controllers
{
    public class HouseholdControllerUnitTests
    {
        private readonly Mock<IHouseholdService> _mockHouseholdService;
        private readonly Mock<IInvitationService> _mockInvitationService;
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly Mock<ILogger<GroupController>> _mockLogger;
        private readonly HouseholdController _controller;

        public HouseholdControllerUnitTests()
        {
            _mockHouseholdService = new Mock<IHouseholdService>();
            _mockInvitationService = new Mock<IInvitationService>();
            _mockAuthService = new Mock<IAuthService>();
            _mockLogger = new Mock<ILogger<GroupController>>();
            _controller = new HouseholdController(_mockHouseholdService.Object, _mockInvitationService.Object, _mockAuthService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllHousehold_ReturnsOkResult_WithListOfHouseholds()
        {
            // Arrange
            var households = new List<HouseholdResponseDto> { new HouseholdResponseDto { HouseholdResponseDtoId = 1, Name = "Test Household", CreatedAt = DateTime.Now, CreatedByUsername = "TestUser" } };
            _mockHouseholdService.Setup(service => service.GetAllHouseholdsAsync()).ReturnsAsync(households);

            // Act
            var result = await _controller.GetAllHousehold();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<HouseholdResponseDto>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetHouseholdById_ReturnsOkResult_WithHousehold()
        {
            // Arrange
            var household = new HouseholdResponseDto { HouseholdResponseDtoId = 1, Name = "Test Household", CreatedAt = DateTime.Now, CreatedByUsername = "TestUser" };
            _mockHouseholdService.Setup(service => service.GetHouseholdByIdAsync(1)).ReturnsAsync(household);

            // Act
            var result = await _controller.GetHouseholdById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<HouseholdResponseDto>(okResult.Value);
            Assert.Equal(1, returnValue.HouseholdResponseDtoId);
        }

        [Fact]
        public async Task CreateHousehold_ReturnsOkResult_WithHouseholdId()
        {
            // Arrange
            var householdDto = new HouseholdDto { HouseholdName = "New Household" };
            var userClaims = new UserClaims { UserId = "1", UserName = "TestUser", Email = "test@example.com", HouseholdId = "1", Roles = new List<string> { "User" } };
            var household = new Household { HouseholdId = 1, Name = "New Household", CreatedAt = DateTime.Now, CreatedByUser = new User { Id = "1", UserName = "TestUser" } };
            _mockAuthService.Setup(service => service.GetClaimsFromHttpContext(It.IsAny<HttpContext>())).Returns(userClaims);
            _mockHouseholdService.Setup(service => service.CreateHouseholdAsync(householdDto, userClaims)).ReturnsAsync(household);

            // Act
            var result = await _controller.CreateHousehold(householdDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<int>(okResult.Value);
            Assert.Equal(1, returnValue);
        }

        [Fact]
        public async Task JoinHousehold_ReturnsOkResult_WithSuccessMessage()
        {
            // Arrange
            var userId = "1";
            var householdId = 1;
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            // Act
            var result = await _controller.JoinHousehold(householdId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains($"User: {userId} has joined household: {householdId}.", okResult.Value.ToString());
        }

        [Fact]
        public async Task UpdateHousehold_ReturnsNoContentResult()
        {
            // Arrange
            var householdDto = new HouseholdDto { HouseholdName = "Updated Household" };

            // Act
            var result = await _controller.UpdateHousehold(1, householdDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteHousehold_ReturnsNoContentResult()
        {
            // Arrange
            _mockHouseholdService.Setup(service => service.DeleteHouseholdAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteHousehold(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}

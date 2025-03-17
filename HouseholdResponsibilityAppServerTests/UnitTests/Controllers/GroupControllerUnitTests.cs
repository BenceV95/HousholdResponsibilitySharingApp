using HouseholdResponsibilityAppServer.Controllers;
using HouseholdResponsibilityAppServer.Models.Groups;
using HouseholdResponsibilityAppServer.Services.Authentication;
using HouseholdResponsibilityAppServer.Services.Groups;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HouseholdResponsibilityAppServerTests.UnitTests.Controllers
{
    public class GroupControllerUnitTests
    {
        private readonly Mock<IGroupService> _mockGroupService;
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly Mock<ILogger<GroupController>> _mockLogger;
        private readonly GroupController _controller;

        public GroupControllerUnitTests()
        {
            _mockGroupService = new Mock<IGroupService>();
            _mockAuthService = new Mock<IAuthService>();
            _mockLogger = new Mock<ILogger<GroupController>>();
            _controller = new GroupController(_mockGroupService.Object, _mockAuthService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllGroup_ReturnsOkResult_WithListOfGroups()
        {
            // Arrange
            var groups = new List<GroupResponseDto> { new GroupResponseDto { GroupResponseDtoId = 1, Name = "Test Group", HouseholdId = 1 } };
            _mockGroupService.Setup(service => service.GetAllGroupsAsync()).ReturnsAsync(groups);

            // Act
            var result = await _controller.GetAllGroup();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<GroupResponseDto>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetGroupById_ReturnsOkResult_WithGroup()
        {
            // Arrange
            var group = new GroupResponseDto { GroupResponseDtoId = 1, Name = "Test Group", HouseholdId = 1 };
            _mockGroupService.Setup(service => service.GetGroupByIdAsync(1)).ReturnsAsync(group);

            // Act
            var result = await _controller.GetGroupById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<GroupResponseDto>(okResult.Value);
            Assert.Equal(1, returnValue.GroupResponseDtoId);
        }

        [Fact]
        public async Task CreateGroup_ReturnsOkResult_WithSuccessMessage()
        {
            // Arrange
            var postGroupDto = new PostGroupDto { GroupName = "New Group" };
            var userClaims = new UserClaims { UserId = "1", UserName = "TestUser", Email = "test@example.com", HouseholdId = "1", Roles = new List<string> { "User" } };
            _mockAuthService.Setup(service => service.GetClaimsFromHttpContext(It.IsAny<HttpContext>())).Returns(userClaims);

            // Act
            var result = await _controller.CreateGroup(postGroupDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Group created successfully", okResult.Value.ToString());
        }

        [Fact]
        public async Task UpdateGroup_ReturnsNoContentResult()
        {
            // Arrange
            var groupDto = new GroupDto { Name = "Updated Group" };
            var userClaims = new UserClaims { UserId = "1", UserName = "TestUser", Email = "test@example.com", HouseholdId = "1", Roles = new List<string> { "User" } };
            _mockAuthService.Setup(service => service.GetClaimsFromHttpContext(It.IsAny<HttpContext>())).Returns(userClaims);

            // Act
            var result = await _controller.UpdateGroup(1, groupDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteGroup_ReturnsNoContentResult()
        {
            // Arrange
            _mockGroupService.Setup(service => service.DeleteGroupAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteGroup(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetGroupsByHouseholdID_ReturnsOkResult_WithListOfGroups()
        {
            // Arrange
            var groups = new List<GroupResponseDto> { new GroupResponseDto { GroupResponseDtoId = 1, Name = "Test Group", HouseholdId = 1 } };
            var userClaims = new UserClaims
            {
                UserId = "1",
                UserName = "TestUser",
                Email = "test@example.com",
                HouseholdId = "1",
                Roles = new List<string> { "User" }
            };
            _mockAuthService.Setup(service => service.GetClaimsFromHttpContext(It.IsAny<HttpContext>())).Returns(userClaims);
            _mockGroupService.Setup(service => service.GetGroupsByHouseholdIdAsync(userClaims)).ReturnsAsync(groups);

            // Act
            var result = await _controller.GetGroupsByHouseholdID();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<GroupResponseDto>>(okResult.Value);
            Assert.Single(returnValue);
        }
    }
}

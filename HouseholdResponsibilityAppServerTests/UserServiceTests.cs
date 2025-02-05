using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseholdResponsibilityAppServer.DTOs;
using HouseholdResponsibilityAppServer.Models;
using HouseholdResponsibilityAppServer.Repositories;
using HouseholdResponsibilityAppServer.Services;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NUnit.Framework;

[TestFixture]
public class UserServiceTests
{
    private IUserRepository _userRepository;
    private UserService _userService;

    [SetUp]
    public void Setup()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _userService = new UserService(_userRepository);
    }

    [Test]
    public async Task GetAllUsersAsync_ShouldReturnUserResponseDtos()
    {
        var users = new List<User>
        {
            new User { UserId = 1, Username = "testuser", Email = "test@example.com", 
                FirstName = "Test", LastName = "User", IsAdmin = false, CreatedAt = DateTime.UtcNow }
        };

        _userRepository.GetAllUsersAsync().Returns(users);

        var result = await _userService.GetAllUsersAsync();

        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result.First().Username, Is.EqualTo("testuser"));
    }

    [Test]
    public async Task GetUserByIdAsync_ShouldReturnUserResponseDto()
    {
        var user = new User { UserId = 1, Username = "testuser", Email = "test@example.com", 
            FirstName = "Test", LastName = "User", IsAdmin = false, CreatedAt = DateTime.UtcNow };

        _userRepository.GetUserByIdAsync(1).Returns(user);

        var result = await _userService.GetUserByIdAsync(1);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Username, Is.EqualTo("testuser"));
    }

    [Test]
    public async Task CreateUserAsync_ShouldCallRepositoryWithHashedPassword()
    {
        var userDto = new UserDto { Username = "newuser", Email = "new@example.com",
            FirstName = "New", LastName = "User", Password = "password123" };

        User createdUser = null;

        _userRepository.When(x => x.AddUserAsync(Arg.Any<User>())) //amikor meghívjuk akk elmentjük a createdUser-be az usert.
            .Do(callInfo => createdUser = callInfo.Arg<User>());

        await _userService.CreateUserAsync(userDto);

        Assert.That(createdUser, Is.Not.Null);
        Assert.That(createdUser.Username, Is.EqualTo("newuser"));
        Assert.That(createdUser.PasswordHash, Is.Not.EqualTo("password123")); 
    }

    [Test]
    public async Task UpdateUserAsync_ShouldUpdateUserProperties()
    {
        var existingUser = new User { UserId = 1, Username = "olduser", Email = "old@example.com", FirstName = "Old", 
            LastName = "User", PasswordHash = "oldhash" };

        var updatedUserDto = new UserDto { Username = "updateduser", Email = "updated@example.com", FirstName = "Updated",
            LastName = "User", Password = "newpassword" };

        _userRepository.GetUserByIdAsync(1).Returns(existingUser);

        await _userService.UpdateUserAsync(1, updatedUserDto);

        Assert.That(existingUser.Username, Is.EqualTo("updateduser"));
        Assert.That(existingUser.PasswordHash, Is.Not.EqualTo("newpassword"));
    }

    [Test]
    public async Task DeleteUserAsync_Should_Call_Repository_DeleteUserAsync()
    {
        int userId = 1;

        await _userService.DeleteUserAsync(userId);

        await _userRepository.Received(1).DeleteUserAsync(userId);
    }
}

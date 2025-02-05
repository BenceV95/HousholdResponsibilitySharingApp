using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseholdResponsibilityAppServer.Data;
using HouseholdResponsibilityAppServer.Models;
using HouseholdResponsibilityAppServer.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

[TestFixture]
public class UserRepositoryTests
{
    private HouseholdResponsibilityContext _context;
    private UserRepository _userRepository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<HouseholdResponsibilityContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
            .Options;

        _context = new HouseholdResponsibilityContext(options);
        _context.Database.EnsureCreated();
        _userRepository = new UserRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task GetAllUsersAsync_ShouldReturnAllUsers()
    {
        _context.Users.AddRange(new List<User>
        {
            new User { UserId = 1, Username = "user1", Email = "user1@example.com", FirstName = "Test", 
                LastName = "User", PasswordHash = "hashedpassword1" },

            new User { UserId = 2, Username = "user2", Email = "user2@example.com", FirstName = "Test", 
                LastName = "User", PasswordHash = "hashedpassword2" }
        });
        await _context.SaveChangesAsync();

        var result = await _userRepository.GetAllUsersAsync();

        Assert.That(result, Has.Exactly(2).Items);
    }

    [Test]
    public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        var user = new User { UserId = 1, Username = "testuser", Email = "test@example.com", FirstName = "Test", 
            LastName = "User", PasswordHash = "hashedpassword123" };

        await _userRepository.AddUserAsync(user);
        await _context.SaveChangesAsync();

        var result = await _userRepository.GetUserByIdAsync(1);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Username, Is.EqualTo("testuser"));
    }

    [Test]
    public async Task GetUserByIdAsync_ShouldThrowExpectedExceptions_WhenUserDoesNotExist()
    {
        var exception = Assert.ThrowsAsync<Exception>(async () =>
            await _userRepository.GetUserByIdAsync(99));

        Assert.That(exception, Is.InstanceOf<KeyNotFoundException>().Or.InstanceOf<Exception>());
        Assert.That(exception.Message, Does.Contain("User with ID 99 not found").
            Or.Contain("Database error: Unable to fetch user by ID."));
    }

    [Test]
    public async Task AddUserAsync_ShouldAddUserToDatabase()
    {
        var user = new User { UserId = 1, Username = "newuser", Email = "new@example.com", FirstName = "Test", 
            LastName = "User", PasswordHash = "hashedpassword123" };

        await _userRepository.AddUserAsync(user);
        await _context.SaveChangesAsync();
        var result = await _context.Users.FindAsync(1);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Username, Is.EqualTo("newuser"));
    }

    [Test]
    public async Task UpdateUserAsync_ShouldUpdateExistingUser()
    {
        var user = new User { UserId = 1, Username = "olduser", Email = "old@example.com", FirstName = "Old", 
            LastName = "User", PasswordHash = "oldhash" };

        await _userRepository.AddUserAsync(user);
        await _context.SaveChangesAsync();

        user.Username = "updateduser";

        await _userRepository.UpdateUserAsync(user);
        await _context.SaveChangesAsync();

        var result = await _userRepository.GetUserByIdAsync(1);

        Assert.That(result.Username, Is.EqualTo("updateduser"));
    }

    [Test]
    public async Task DeleteUserAsync_ShouldRemoveUserFromDatabase()
    {
        var user = new User { UserId = 1, Username = "user1", Email = "user1@example.com", FirstName = "Test", 
            LastName = "User", PasswordHash = "hashedpassword123" };

        await _userRepository.AddUserAsync(user);
        await _context.SaveChangesAsync();

        await _userRepository.DeleteUserAsync(1);
        await _context.SaveChangesAsync();

        var users = await _userRepository.GetAllUsersAsync();
        Assert.That(users, Is.Empty);
    }
}
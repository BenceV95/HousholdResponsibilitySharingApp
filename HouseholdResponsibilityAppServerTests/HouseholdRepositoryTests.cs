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
public class HouseholdRepositoryTests
{
    private HouseholdResponsibilityContext _context;
    private HouseholdRepository _householdRepository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<HouseholdResponsibilityContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
            .Options;

        _context = new HouseholdResponsibilityContext(options);
        _context.Database.EnsureCreated();
        _householdRepository = new HouseholdRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task GetAllHouseholdsAsync_ShouldReturnAllHouseholds()
    {
        _context.Households.AddRange(new List<Household>
        {
            new Household { HouseholdId = 1, Name = "Household1", CreatedAt = DateTime.UtcNow },
            new Household { HouseholdId = 2, Name = "Household2", CreatedAt = DateTime.UtcNow }
        });
        await _context.SaveChangesAsync();

        var result = await _householdRepository.GetAllHouseholdsAsync();

        Assert.That(result, Has.Exactly(2).Items);
    }

    [Test]
    public async Task GetHouseholdByIdAsync_ShouldReturnHousehold_WhenHouseholdExists()
    {
        var household = new Household { HouseholdId = 1, Name = "TestHousehold", CreatedAt = DateTime.UtcNow };

        await _householdRepository.AddHouseholdAsync(household);
        await _context.SaveChangesAsync();

        var result = await _householdRepository.GetHouseholdByIdAsync(1);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("TestHousehold"));
    }

    [Test]
    public async Task GetHouseholdByIdAsync_ShouldThrowExpectedExceptions_WhenHouseholdDoesNotExist()
    {
        var exception = Assert.ThrowsAsync<Exception>(async () =>
            await _householdRepository.GetHouseholdByIdAsync(99));

        Assert.That(exception, Is.InstanceOf<KeyNotFoundException>().Or.InstanceOf<Exception>());
        Assert.That(exception.Message, Does.Contain("Household with ID 99 not found")
            .Or.Contain("Database error: Unable to fetch household by ID."));
    }

    [Test]
    public async Task AddHouseholdAsync_ShouldAddHouseholdToDatabase()
    {
        var household = new Household { HouseholdId = 1, Name = "NewHousehold", CreatedAt = DateTime.UtcNow };

        await _householdRepository.AddHouseholdAsync(household);
        await _context.SaveChangesAsync();
        var result = await _context.Households.FindAsync(1);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("NewHousehold"));
    }

    [Test]
    public async Task UpdateHouseholdAsync_ShouldUpdateExistingHousehold()
    {
        var household = new Household { HouseholdId = 1, Name = "OldHousehold", CreatedAt = DateTime.UtcNow };
        await _householdRepository.AddHouseholdAsync(household);
        await _context.SaveChangesAsync();

        household.Name = "UpdatedHousehold";

        await _householdRepository.UpdateHouseholdAsync(household);
        await _context.SaveChangesAsync();
        var result = await _householdRepository.GetHouseholdByIdAsync(1);

        Assert.That(result.Name, Is.EqualTo("UpdatedHousehold"));
    }

    [Test]
    public async Task DeleteHouseholdAsync_ShouldRemoveHouseholdFromDatabase()
    {
        var household = new Household { HouseholdId = 1, Name = "HouseholdToDelete", CreatedAt = DateTime.UtcNow };
        await _householdRepository.AddHouseholdAsync(household);
        await _context.SaveChangesAsync();

        await _householdRepository.DeleteHouseholdAsync(1);
        await _context.SaveChangesAsync();

        var households = await _householdRepository.GetAllHouseholdsAsync();
        Assert.That(households, Is.Empty);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseholdResponsibilityAppServer.DTOs;
using HouseholdResponsibilityAppServer.Models;
using HouseholdResponsibilityAppServer.Repositories;
using HouseholdResponsibilityAppServer.Services;
using NSubstitute;
using NUnit.Framework;

[TestFixture]
public class HouseholdServiceTests
{
    private IHouseholdRepository _householdRepository;
    private HouseholdService _householdService;

    [SetUp]
    public void Setup()
    {
        _householdRepository = Substitute.For<IHouseholdRepository>();
        _householdService = new HouseholdService(_householdRepository);
    }

    [Test]
    public async Task GetAllHouseholdsAsync_ShouldReturnHouseholdResponseDtos()
    {
        var households = new List<Household>
        {
            new Household { HouseholdId = 1, Name = "Test Household", CreatedAt = DateTime.UtcNow, 
                CreatedByUser = new User { Username = "creator" } }
        };

        _householdRepository.GetAllHouseholdsAsync().Returns(households);

        var result = await _householdService.GetAllHouseholdsAsync();

        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result.First().Name, Is.EqualTo("Test Household"));
        Assert.That(result.First().CreatedByUsername, Is.EqualTo("creator"));
    }

    [Test]
    public async Task GetHouseholdByIdAsync_ShouldReturnHouseholdResponseDto()
    {
        var household = new Household { HouseholdId = 1, Name = "Test Household", CreatedAt = DateTime.UtcNow, 
            CreatedByUser = new User { Username = "creator" } };

        _householdRepository.GetHouseholdByIdAsync(1).Returns(household);

        var result = await _householdService.GetHouseholdByIdAsync(1);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("Test Household"));
        Assert.That(result.CreatedByUsername, Is.EqualTo("creator"));
    }

    [Test]
    public async Task CreateHouseholdAsync_ShouldCallRepository()
    {
        var householdDto = new HouseholdDto { Name = "New Household" };

        Household createdHousehold = null;

        _householdRepository.When(x => x.AddHouseholdAsync(Arg.Any<Household>()))
            .Do(callInfo => createdHousehold = callInfo.Arg<Household>());

        await _householdService.CreateHouseholdAsync(householdDto);

        Assert.That(createdHousehold, Is.Not.Null);
        Assert.That(createdHousehold.Name, Is.EqualTo("New Household"));
    }

    [Test]
    public async Task UpdateHouseholdAsync_ShouldUpdateHouseholdProperties()
    {
        var existingHousehold = new Household { HouseholdId = 1, Name = "Old Household" };

        var updatedHouseholdDto = new HouseholdDto { Name = "Updated Household" };

        _householdRepository.GetHouseholdByIdAsync(1).Returns(existingHousehold);

        await _householdService.UpdateHouseholdAsync(1, updatedHouseholdDto);

        Assert.That(existingHousehold.Name, Is.EqualTo("Updated Household"));
    }

    [Test]
    public async Task DeleteHouseholdAsync_ShouldCallRepository()
    {
        int householdId = 1;

        await _householdService.DeleteHouseholdAsync(householdId);

        await _householdRepository.Received(1).DeleteHouseholdAsync(householdId);
    }
}

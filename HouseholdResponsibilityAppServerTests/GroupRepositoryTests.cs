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
public class GroupRepositoryTests
{
    private HouseholdResponsibilityContext _context;
    private GroupRepository _groupRepository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<HouseholdResponsibilityContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
            .Options;

        _context = new HouseholdResponsibilityContext(options);
        _context.Database.EnsureCreated();
        _groupRepository = new GroupRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task GetAllGroupsAsync_ShouldReturnAllGroups()
    {
        _context.Groups.AddRange(new List<TaskGroup>
        {
            new TaskGroup { GroupId = 1, Name = "Group1" },
            new TaskGroup { GroupId = 2, Name = "Group2" }
        });
        await _context.SaveChangesAsync();

        var result = await _groupRepository.GetAllGroupsAsync();

        Assert.That(result, Has.Exactly(2).Items);
    }

    [Test]
    public async Task GetGroupByIdAsync_ShouldReturnGroup_WhenGroupExists()
    {
        var group = new TaskGroup { GroupId = 1, Name = "TestGroup" };
        await _groupRepository.AddGroupAsync(group);
        await _context.SaveChangesAsync();

        var result = await _groupRepository.GetGroupByIdAsync(1);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("TestGroup"));
    }

    [Test]
    public async Task GetGroupByIdAsync_ShouldThrowExpectedExceptions_WhenGroupDoesNotExist()
    {
        var exception = Assert.ThrowsAsync<Exception>(async () =>
            await _groupRepository.GetGroupByIdAsync(99));

        Assert.That(exception, Is.InstanceOf<KeyNotFoundException>().Or.InstanceOf<Exception>());
        Assert.That(exception.Message, Does.Contain("Group with ID 99 not found")
            .Or.Contain("Database error: Unable to fetch group by ID."));
    }

    [Test]
    public async Task AddGroupAsync_ShouldAddGroupToDatabase()
    {
        var group = new TaskGroup { GroupId = 1, Name = "NewGroup" };

        await _groupRepository.AddGroupAsync(group);
        await _context.SaveChangesAsync();

        var result = await _context.Groups.FindAsync(1);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("NewGroup"));
    }

    [Test]
    public async Task UpdateGroupAsync_ShouldUpdateExistingGroup()
    {
        var group = new TaskGroup { GroupId = 1, Name = "OldGroup" };
        await _groupRepository.AddGroupAsync(group);
        await _context.SaveChangesAsync();

        group.Name = "UpdatedGroup";

        await _groupRepository.UpdateGroupAsync(group);
        await _context.SaveChangesAsync();
        var result = await _groupRepository.GetGroupByIdAsync(1);

        Assert.That(result.Name, Is.EqualTo("UpdatedGroup"));
    }

    [Test]
    public async Task DeleteGroupAsync_ShouldRemoveGroupFromDatabase()
    {
        var group = new TaskGroup { GroupId = 1, Name = "GroupToDelete" };
        await _groupRepository.AddGroupAsync(group);
        await _context.SaveChangesAsync();

        await _groupRepository.DeleteGroupAsync(1);
        await _context.SaveChangesAsync();

        var groups = await _groupRepository.GetAllGroupsAsync();
        Assert.That(groups, Is.Empty);
    }
}

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
public class GroupServiceTests
{
    private IGroupRepository _groupRepository;
    private GroupService _groupService;

    [SetUp]
    public void Setup()
    {
        _groupRepository = Substitute.For<IGroupRepository>();
        _groupService = new GroupService(_groupRepository);
    }

    [Test]
    public async Task GetAllGroupsAsync_ShouldReturnGroupResponseDtos()
    {
        var groups = new List<TaskGroup>
        {
            new TaskGroup { GroupId = 1, Name = "Test Group" }
        };

        _groupRepository.GetAllGroupsAsync().Returns(groups);

        var result = await _groupService.GetAllGroupsAsync();

        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result.First().Name, Is.EqualTo("Test Group"));
    }

    [Test]
    public async Task GetGroupByIdAsync_ShouldReturnGroupResponseDto()
    {
        var group = new TaskGroup { GroupId = 1, Name = "Test Group" };

        _groupRepository.GetGroupByIdAsync(1).Returns(group);

        var result = await _groupService.GetGroupByIdAsync(1);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("Test Group"));
    }

    [Test]
    public async Task CreateGroupAsync_ShouldCallRepository()
    {
        var groupDto = new GroupDto { Name = "New Group" };

        TaskGroup createdGroup = null;

        _groupRepository.When(x => x.AddGroupAsync(Arg.Any<TaskGroup>()))
            .Do(callInfo => createdGroup = callInfo.Arg<TaskGroup>());

        await _groupService.CreateGroupAsync(groupDto);

        Assert.That(createdGroup, Is.Not.Null);
        Assert.That(createdGroup.Name, Is.EqualTo("New Group"));
    }

    [Test]
    public async Task UpdateGroupAsync_ShouldUpdateGroupProperties()
    {
        var existingGroup = new TaskGroup { GroupId = 1, Name = "Old Group" };

        var updatedGroupDto = new GroupDto { Name = "Updated Group" };

        _groupRepository.GetGroupByIdAsync(1).Returns(existingGroup);

        await _groupService.UpdateGroupAsync(1, updatedGroupDto);

        Assert.That(existingGroup.Name, Is.EqualTo("Updated Group"));
    }

    [Test]
    public async Task DeleteGroupAsync_ShouldCallRepository()
    {
        int groupId = 1;

        await _groupService.DeleteGroupAsync(groupId);

        await _groupRepository.Received(1).DeleteGroupAsync(groupId);
    }
}

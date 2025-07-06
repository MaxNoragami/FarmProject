using FarmProject.Application.UnitTests.Mocks;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;
using FluentAssertions;
using FarmTaskServiceClass = FarmProject.Application.FarmTaskService.FarmTaskService;

namespace FarmProject.Application.UnitTests;

public class FarmTaskServiceTests
{
    [Fact]
    public async Task GetFarmTaskById_TaskExists_ReturnsTask()
    {
        var mockFarmTaskRepo = new MockFarmTaskRepository();
        var expectedTask = new FarmTask(
            FarmTaskType.NestPreparation,
            "Test task",
            DateTime.Now,
            DateTime.Now.AddDays(1))
        {
            Id = 1
        };

        await mockFarmTaskRepo.AddAsync(expectedTask);

        var mockUnitOfWork = new MockUnitOfWork(farmTaskRepository: mockFarmTaskRepo);
        var farmTaskService = new FarmTaskServiceClass(mockUnitOfWork, null);

        var result = await farmTaskService.GetFarmTaskById(1);

        Assert.True(result.IsSuccess);
        result.Value.Should().BeEquivalentTo(expectedTask);
    }

    [Fact]
    public async Task MarkFarmTaskAsCompleted_TaskExists_CompletesTask()
    {
        var mockFarmTaskRepo = new MockFarmTaskRepository();
        var task = new FarmTask(
            FarmTaskType.NestPreparation,
            "Test task",
            DateTime.Now,
            DateTime.Now.AddDays(1))
        {
            Id = 1
        };

        await mockFarmTaskRepo.AddAsync(task);

        var mockUnitOfWork = new MockUnitOfWork(farmTaskRepository: mockFarmTaskRepo);
        var farmTaskService = new FarmTaskServiceClass(mockUnitOfWork, null);

        var result = await farmTaskService.MarkFarmTaskAsCompleted(1);

        Assert.True(result.IsSuccess);
        Assert.True(result.Value.IsCompleted);
        result.Value.Should().BeEquivalentTo(task);
    }
}

using Moq;
using FluentAssertions;
using TodoApp.Application.Abstractions;
using TodoApp.Application.UseCases.Queries;
using TodoApp.Domain.Models;

namespace TodoApp.UnitTests.Application.UseCases.Queries;

public class GetTodoByIdQueryTests
{
    private readonly Mock<ITodoRepository> _mockRepository;
    private readonly GetTodoByIdQuery _query;

    public GetTodoByIdQueryTests()
    {
        _mockRepository = new Mock<ITodoRepository>();
        _query = new GetTodoByIdQuery(_mockRepository.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNull_WhenNotFound()
    {
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((TodoItem?)null);
        var result = await _query.ExecuteAsync(1);
        result.Should().BeNull();
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnMappedResponse_WhenFound()
    {
        var todo = new TodoItem { Id = 1, Task = "Test", Category = new TodoCategory { Name = "Cat", Color = "#000" } };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(todo);

        var result = await _query.ExecuteAsync(1);

        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Task.Should().Be("Test");
    }
}

using Moq;
using FluentAssertions;
using TodoApp.Application.Abstractions;
using TodoApp.Application.UseCases.Queries;
using TodoApp.Domain.Models;

namespace TodoApp.UnitTests.Application.UseCases.Queries;

public class GetTodosQueryTests
{
    private readonly Mock<ITodoRepository> _mockRepository;
    private readonly GetTodosQuery _query;

    public GetTodosQueryTests()
    {
        _mockRepository = new Mock<ITodoRepository>();
        _query = new GetTodosQuery(_mockRepository.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnMappedResponses()
    {
        var todos = new List<TodoItem>
        {
            new TodoItem { Id = 1, Task = "Task 1", Category = new TodoCategory { Name = "C1", Color = "#111" } }
        };
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(todos);

        var result = await _query.ExecuteAsync();

        result.Should().HaveCount(1);
        result.First().Id.Should().Be(1);
        result.First().Task.Should().Be("Task 1");
    }
}

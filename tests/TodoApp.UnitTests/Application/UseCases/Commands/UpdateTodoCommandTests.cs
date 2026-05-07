using Moq;
using FluentAssertions;
using TodoApp.Application.Abstractions;
using TodoApp.Application.Contracts.Todos;
using TodoApp.Application.UseCases.Commands;
using TodoApp.Domain.Models;

namespace TodoApp.UnitTests.Application.UseCases.Commands;

public class UpdateTodoCommandTests
{
    private readonly Mock<ITodoRepository> _mockRepository;
    private readonly UpdateTodoCommand _command;

    public UpdateTodoCommandTests()
    {
        _mockRepository = new Mock<ITodoRepository>();
        
        _command = new UpdateTodoCommand(_mockRepository.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldUpdateTodo_WhenValidRequest()
    {
        var request = new UpdateTodoRequest(1, "Updated Task", "Low", new TodoCategoryDto("Work", "#00ff00"));
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<TodoItem>())).ReturnsAsync(true);

        var result = await _command.ExecuteAsync(request);

        result.Should().BeTrue();
        _mockRepository.Verify(r => r.UpdateAsync(It.Is<TodoItem>(t => t.Id == 1 && t.Task == "Updated Task")), Times.Once);
    }
}

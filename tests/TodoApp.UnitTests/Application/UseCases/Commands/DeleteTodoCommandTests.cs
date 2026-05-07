using Moq;
using FluentAssertions;
using TodoApp.Application.Abstractions;
using TodoApp.Application.UseCases.Commands;

namespace TodoApp.UnitTests.Application.UseCases.Commands;

public class DeleteTodoCommandTests
{
    private readonly Mock<ITodoRepository> _mockRepository;
    private readonly DeleteTodoCommand _command;

    public DeleteTodoCommandTests()
    {
        _mockRepository = new Mock<ITodoRepository>();
        _command = new DeleteTodoCommand(_mockRepository.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldCallDeleteOnRepository()
    {
        _mockRepository.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);
        var result = await _command.ExecuteAsync(1);
        result.Should().BeTrue();
        _mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
    }
}

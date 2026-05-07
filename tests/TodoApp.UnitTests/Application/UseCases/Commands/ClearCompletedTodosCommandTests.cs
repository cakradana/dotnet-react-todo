using Moq;
using FluentAssertions;
using TodoApp.Application.Abstractions;
using TodoApp.Application.UseCases.Commands;

namespace TodoApp.UnitTests.Application.UseCases.Commands;

public class ClearCompletedTodosCommandTests
{
    private readonly Mock<ITodoRepository> _mockRepository;
    private readonly ClearCompletedTodosCommand _command;

    public ClearCompletedTodosCommandTests()
    {
        _mockRepository = new Mock<ITodoRepository>();
        _command = new ClearCompletedTodosCommand(_mockRepository.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldCallClearCompletedOnRepository()
    {
        _mockRepository.Setup(r => r.RemoveCompletedAsync()).ReturnsAsync(5);
        var result = await _command.ExecuteAsync();

        result.Should().Be(5);
        _mockRepository.Verify(r => r.RemoveCompletedAsync(), Times.Once);
    }
}

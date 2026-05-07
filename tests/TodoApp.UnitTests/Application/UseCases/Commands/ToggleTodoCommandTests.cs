using Moq;
using FluentAssertions;
using TodoApp.Application.Abstractions;
using TodoApp.Application.UseCases.Commands;
using TodoApp.Domain.Models;

namespace TodoApp.UnitTests.Application.UseCases.Commands;

public class ToggleTodoCommandTests
{
    private readonly Mock<ITodoRepository> _mockRepository;
    private readonly ToggleTodoCommand _command;

    public ToggleTodoCommandTests()
    {
        _mockRepository = new Mock<ITodoRepository>();
        _command = new ToggleTodoCommand(_mockRepository.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldToggleStatusAndSave()
    {
        _mockRepository.Setup(r => r.ToggleCompletedAsync(1)).ReturnsAsync(true);

        var result = await _command.ExecuteAsync(1);

        result.Should().BeTrue();
        _mockRepository.Verify(r => r.ToggleCompletedAsync(1), Times.Once);
    }
}

using Moq;
using FluentAssertions;
using TodoApp.Application.Abstractions;
using TodoApp.Application.Contracts.Todos;
using TodoApp.Application.UseCases.Commands;
using TodoApp.Domain.Models;

namespace TodoApp.UnitTests.Application.UseCases.Commands;

public class CreateTodoCommandTests
{
    private readonly Mock<ITodoRepository> _mockRepository;
    private readonly Mock<FluentValidation.IValidator<CreateTodoRequest>> _mockValidator;
    private readonly CreateTodoCommand _command;

    public CreateTodoCommandTests()
    {
        _mockRepository = new Mock<ITodoRepository>();
        _mockValidator = new Mock<FluentValidation.IValidator<CreateTodoRequest>>();
        
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<FluentValidation.ValidationContext<CreateTodoRequest>>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _command = new CreateTodoCommand(_mockRepository.Object, _mockValidator.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldCreateTodoAndReturnResponse()
    {
        // Arrange
        var request = new CreateTodoRequest(
            "Test Task",
            "High",
            new TodoCategoryDto("Work", "#ff0000")
        );

        _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<TodoItem>()))
            .ReturnsAsync((TodoItem item) => 
            {
                item.Id = 1; // Simulate DB generating an ID
                return item;
            });

        // Act
        var result = await _command.ExecuteAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Task.Should().Be(request.Task);
        result.Priority.Should().Be(request.Priority);
        result.Category.Name.Should().Be(request.Category.Name);
        result.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(2));
        
        _mockRepository.Verify(repo => repo.AddAsync(It.Is<TodoItem>(t => t.Task == request.Task)), Times.Once);
    }
}

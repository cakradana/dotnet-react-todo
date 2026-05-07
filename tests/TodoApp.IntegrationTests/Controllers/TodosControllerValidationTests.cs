using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TodoApp.Application.Contracts.Todos;

namespace TodoApp.IntegrationTests.Controllers;

public class TodosControllerValidationTests : IClassFixture<TodoAppFactory>
{
    private readonly HttpClient _client;

    public TodosControllerValidationTests(TodoAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateTodo_WithEmptyTask_ShouldReturnBadRequest()
    {
        // 1. Arrange - Empty Task
        var request = new CreateTodoRequest(
            "", // Invalid
            "Low",
            new TodoCategoryDto("Test", "#0000ff")
        );

        // 2. Act
        var createResponse = await _client.PostAsJsonAsync("/api/todos", request);
        
        // 3. Assert
        createResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateTodo_WithWhitespaceTask_ShouldReturnBadRequest()
    {
        var request = new CreateTodoRequest(
            "   ", // Invalid whitespace
            "Low",
            new TodoCategoryDto("Test", "#0000ff")
        );

        var createResponse = await _client.PostAsJsonAsync("/api/todos", request);
        createResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateTodo_WithInvalidPriority_ShouldReturnBadRequest()
    {
        var request = new CreateTodoRequest(
            "Valid Task",
            "UnknownPriority", // Invalid
            new TodoCategoryDto("Test", "#0000ff")
        );

        var createResponse = await _client.PostAsJsonAsync("/api/todos", request);
        createResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateTodo_WithInvalidTaskOrPriority_ShouldReturnBadRequest()
    {
        var request = new UpdateTodoRequest(
            1,
            "   ", // Invalid
            "Low",
            new TodoCategoryDto("Test", "#0000ff")
        );

        var updateResponse = await _client.PutAsJsonAsync("/api/todos/1", request);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var request2 = new UpdateTodoRequest(
            1,
            "Valid Task",
            "UnknownPriority", // Invalid
            new TodoCategoryDto("Test", "#0000ff")
        );

        var updateResponse2 = await _client.PutAsJsonAsync("/api/todos/1", request2);
        updateResponse2.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}

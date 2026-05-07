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
}

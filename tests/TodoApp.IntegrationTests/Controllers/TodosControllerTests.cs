using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TodoApp.Application.Contracts.Todos;
using TodoApp.Domain.Models;

namespace TodoApp.IntegrationTests.Controllers;

public class TodosControllerTests : IClassFixture<TodoAppFactory>
{
    private readonly HttpClient _client;

    public TodosControllerTests(TodoAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task FullCrud_IntegrationTest_ShouldWorkEndToEnd()
    {
        // 1. Arrange - Create
        var request = new CreateTodoRequest("Full CRUD Task", "Medium", new TodoCategoryDto("Test", "#0000ff"));

        // 2. Act - Create
        var createResponse = await _client.PostAsJsonAsync("/api/todos", request);
        var content = await createResponse.Content.ReadAsStringAsync();
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created, content);
        var createdTodo = await createResponse.Content.ReadFromJsonAsync<TodoResponse>();
        createdTodo.Should().NotBeNull();
        var todoId = createdTodo!.Id;

        // 3. Act - Get By Id
        var getByIdResponse = await _client.GetAsync($"/api/todos/{todoId}");
        getByIdResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var fetchedTodo = await getByIdResponse.Content.ReadFromJsonAsync<TodoResponse>();
        fetchedTodo!.Task.Should().Be("Full CRUD Task");

        // 4. Act - Update
        var updateRequest = new UpdateTodoRequest(todoId, "Updated Text", "High", new TodoCategoryDto("Work", "#ff0000"));
        var updateResponse = await _client.PutAsJsonAsync($"/api/todos/{todoId}", updateRequest);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var verifyUpdateResponse = await _client.GetFromJsonAsync<TodoResponse>($"/api/todos/{todoId}");
        verifyUpdateResponse.Should().NotBeNull();
        verifyUpdateResponse!.Task.Should().Be("Updated Text");
        verifyUpdateResponse.Priority.Should().Be("High");
        verifyUpdateResponse.Category.Name.Should().Be("Work");

        // 5. Act - Toggle
        var toggleResponse = await _client.PatchAsync($"/api/todos/{todoId}/toggle", null);
        toggleResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var verifyToggleResponse = await _client.GetFromJsonAsync<TodoResponse>($"/api/todos/{todoId}");
        verifyToggleResponse.Should().NotBeNull();
        verifyToggleResponse!.IsCompleted.Should().BeTrue();

        // 6. Act - Delete
        var deleteResponse = await _client.DeleteAsync($"/api/todos/{todoId}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // 7. Act - Verify Deleted
        var getDeletedResponse = await _client.GetAsync($"/api/todos/{todoId}");
        getDeletedResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}

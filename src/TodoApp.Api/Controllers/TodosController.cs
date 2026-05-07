using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.UseCases.Commands;
using TodoApp.Application.UseCases.Queries;
using TodoApp.Application.Contracts.Todos;

namespace TodoApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly GetTodosQuery getTodosQuery;
    private readonly GetTodoByIdQuery getTodoByIdQuery;
    private readonly CreateTodoCommand createTodoCommand;
    private readonly UpdateTodoCommand updateTodoCommand;
    private readonly DeleteTodoCommand deleteTodoCommand;
    private readonly ToggleTodoCommand toggleTodoCommand;
    private readonly ClearCompletedTodosCommand clearCompletedCommand;

    /// <summary>
    /// Initializes a new <see cref="TodosController"/> with the required use-case dependencies.
    /// </summary>
    /// <param name="getTodosQuery">Retrieves all todos.</param>
    /// <param name="getTodoByIdQuery">Retrieves a single todo by its identifier.</param>
    /// <param name="createTodoCommand">Creates a new todo.</param>
    /// <param name="updateTodoCommand">Updates an existing todo.</param>
    /// <param name="deleteTodoCommand">Deletes a todo by its identifier.</param>
    /// <param name="toggleTodoCommand">Toggles the completion state of a todo.</param>
    /// <param name="clearCompletedCommand">Clears all completed todos.</param>
    public TodosController(
        GetTodosQuery getTodosQuery,
        GetTodoByIdQuery getTodoByIdQuery,
        CreateTodoCommand createTodoCommand,
        UpdateTodoCommand updateTodoCommand,
        DeleteTodoCommand deleteTodoCommand,
        ToggleTodoCommand toggleTodoCommand,
        ClearCompletedTodosCommand clearCompletedCommand)
    {
        this.getTodosQuery = getTodosQuery;
        this.getTodoByIdQuery = getTodoByIdQuery;
        this.createTodoCommand = createTodoCommand;
        this.updateTodoCommand = updateTodoCommand;
        this.deleteTodoCommand = deleteTodoCommand;
        this.toggleTodoCommand = toggleTodoCommand;
        this.clearCompletedCommand = clearCompletedCommand;
    }

    /// <summary>
    /// Retrieves all todo items.
    /// </summary>
    /// <returns>A list of <see cref="TodoResponse"/> objects representing all todos.</returns>
    [HttpGet]
    public async Task<ActionResult<List<TodoResponse>>> GetTodos()
    {
        var todos = await getTodosQuery.ExecuteAsync();
        return Ok(todos);
    }

    /// <summary>
    /// Retrieves a todo by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the todo to retrieve.</param>
    /// <returns>The requested <see cref="TodoResponse"/> when found; otherwise a NotFound result.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoResponse>> GetTodoById(long id)
    {
        var todo = await getTodoByIdQuery.ExecuteAsync(id);
        if (todo is null)
        {
            return NotFound();
        }

        return Ok(todo);
    }

    /// <summary>
    /// Creates a new todo from the provided request and returns the created resource.
    /// </summary>
    /// <param name="request">The details of the todo to create.</param>
    /// <returns>The created <see cref="TodoResponse"/> and a 201 Created response with a Location header for the new resource.</returns>
    [HttpPost]
    public async Task<ActionResult<TodoResponse>> CreateTodo([FromBody] CreateTodoRequest request)
    {
        var response = await createTodoCommand.ExecuteAsync(request);
        return CreatedAtAction(nameof(GetTodoById), new { id = response.Id }, response);
    }

    /// <summary>
    /// Updates an existing todo with the values from the provided request.
    /// </summary>
    /// <param name="id">The id from the route; must match <c>request.Id</c>.</param>
    /// <param name="request">The updated todo data.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> representing the outcome:
    /// `400 BadRequest` if the route id and payload id differ; `404 NotFound` if the todo does not exist; `204 NoContent` on successful update.
    /// </returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTodo(long id, [FromBody] UpdateTodoRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest("ID routing and payload mismatch.");
        }

        var success = await updateTodoCommand.ExecuteAsync(request);
        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Deletes the todo item with the specified id.
    /// </summary>
    /// <param name="id">The identifier of the todo to delete.</param>
    /// <returns>An <see cref="IActionResult"/> that is 204 NoContent if the todo was deleted, or 404 NotFound if no todo with the specified id exists.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodo(long id)
    {
        var success = await deleteTodoCommand.ExecuteAsync(id);
        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Toggles the completion status of the specified todo.
    /// </summary>
    /// <param name="id">The identifier of the todo to toggle.</param>
    /// <returns>`204 NoContent` if the todo was toggled successfully; `404 NotFound` if no todo with the specified id exists.</returns>
    [HttpPatch("{id}/toggle")]
    public async Task<IActionResult> ToggleTodo(long id)
    {
        var success = await toggleTodoCommand.ExecuteAsync(id);
        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Removes all completed todos and reports how many were removed.
    /// </summary>
    /// <returns>HTTP 200 OK with an object containing `Count` (the number of todos removed) and `Message` (a human-readable summary).</returns>
    [HttpDelete("completed")]
    public async Task<IActionResult> ClearCompleted()
    {
        var count = await clearCompletedCommand.ExecuteAsync();
        return Ok(new { Count = count, Message = $"{count} completed todos removed." });
    }
}
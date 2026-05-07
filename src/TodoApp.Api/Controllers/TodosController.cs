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

    [HttpGet]
    public async Task<ActionResult<List<TodoResponse>>> GetTodos()
    {
        var todos = await getTodosQuery.ExecuteAsync();
        return Ok(todos);
    }

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

    [HttpPost]
    public async Task<ActionResult<TodoResponse>> CreateTodo([FromBody] CreateTodoRequest request)
    {
        var response = await createTodoCommand.ExecuteAsync(request);
        return CreatedAtAction(nameof(GetTodoById), new { id = response.Id }, response);
    }

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

    [HttpDelete("completed")]
    public async Task<IActionResult> ClearCompleted()
    {
        var count = await clearCompletedCommand.ExecuteAsync();
        return Ok(new { Count = count, Message = $"{count} completed todos removed." });
    }
}
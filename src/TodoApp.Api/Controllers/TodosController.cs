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
    public ActionResult<List<TodoResponse>> GetTodos()
    {
        var todos = getTodosQuery.Execute();
        return Ok(todos);
    }

    [HttpGet("{id}")]
    public ActionResult<TodoResponse> GetTodoById(long id)
    {
        var todo = getTodoByIdQuery.Execute(id);
        if (todo is null)
        {
            return NotFound();
        }

        return Ok(todo);
    }

    [HttpPost]
    public ActionResult<TodoResponse> CreateTodo([FromBody] CreateTodoRequest request)
    {
        var response = createTodoCommand.Execute(request);
        return CreatedAtAction(nameof(GetTodoById), new { id = response.Id }, response);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateTodo(long id, [FromBody] UpdateTodoRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest("ID routing and payload mismatch.");
        }

        var success = updateTodoCommand.Execute(request);
        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteTodo(long id)
    {
        var success = deleteTodoCommand.Execute(id);
        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPatch("{id}/toggle")]
    public IActionResult ToggleTodo(long id)
    {
        var success = toggleTodoCommand.Execute(id);
        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("completed")]
    public IActionResult ClearCompleted()
    {
        var count = clearCompletedCommand.Execute();
        return Ok(new { Count = count, Message = $"{count} completed todos removed." });
    }
}
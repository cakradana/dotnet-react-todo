using TodoApp.Application.Abstractions;
using TodoApp.Application.Contracts.Todos;
using TodoApp.Domain.Models;

namespace TodoApp.Application.UseCases.Queries;

public sealed class GetTodosQuery
{
    private readonly ITodoRepository todoRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetTodosQuery"/> class with the specified todo repository.
    /// </summary>
    /// <param name="todoRepository">Repository used to retrieve todo items.</param>
    public GetTodosQuery(ITodoRepository todoRepository)
    {
        this.todoRepository = todoRepository;
    }

    /// <summary>
    /// Retrieves all todo items from the repository and maps them to <see cref="TodoResponse"/> DTOs.
    /// </summary>
    /// <returns>A list of <see cref="TodoResponse"/> objects representing all todos.</returns>
    public async Task<List<TodoResponse>> ExecuteAsync()
    {
        var todos = await todoRepository.GetAllAsync();
        return todos.Select(MapToResponse).ToList();
    }

    /// <summary>
    /// Converts a <see cref="TodoItem"/> entity into a <see cref="TodoResponse"/> DTO.
    /// </summary>
    /// <param name="item">The source <see cref="TodoItem"/> to map.</param>
    /// <returns>A <see cref="TodoResponse"/> populated with the item's Id, Task, IsCompleted, Priority, CreatedAt, and a <see cref="TodoCategoryDto"/> created from the item's category Name and Color.</returns>
    private static TodoResponse MapToResponse(TodoItem item)
    {
        return new TodoResponse(
            item.Id,
            item.Task,
            item.IsCompleted,
            item.Priority,
            item.CreatedAt,
            new TodoCategoryDto(item.Category.Name, item.Category.Color));
    }
}
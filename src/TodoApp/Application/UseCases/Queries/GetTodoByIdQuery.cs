using TodoApp.Application.Abstractions;
using TodoApp.Application.Contracts.Todos;
using TodoApp.Domain.Models;

namespace TodoApp.Application.UseCases.Queries;

public sealed class GetTodoByIdQuery
{
    private readonly ITodoRepository todoRepository;

    /// <summary>
    /// Initializes a new instance of <see cref="GetTodoByIdQuery"/> with the specified repository.
    /// </summary>
    /// <param name="todoRepository">Repository used to retrieve todo entities by identifier.</param>
    public GetTodoByIdQuery(ITodoRepository todoRepository)
    {
        this.todoRepository = todoRepository;
    }

    /// <summary>
    /// Retrieves a todo by its identifier and maps it to a <see cref="TodoResponse"/>.
    /// </summary>
    /// <param name="id">The identifier of the todo to fetch.</param>
    /// <returns>A <see cref="TodoResponse"/> for the specified todo, or <c>null</c> if no todo with the given id exists.</returns>
    public async Task<TodoResponse?> ExecuteAsync(long id)
    {
        var item = await todoRepository.GetByIdAsync(id);
        if (item is null)
        {
            return null;
        }

        return new TodoResponse(
            item.Id,
            item.Task,
            item.IsCompleted,
            item.Priority,
            item.CreatedAt,
            new TodoCategoryDto(item.Category.Name, item.Category.Color));
    }
}
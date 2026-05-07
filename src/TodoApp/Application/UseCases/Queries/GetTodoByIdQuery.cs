using TodoApp.Application.Abstractions;
using TodoApp.Application.Contracts.Todos;
using TodoApp.Domain.Models;

namespace TodoApp.Application.UseCases.Queries;

public sealed class GetTodoByIdQuery
{
    private readonly ITodoRepository todoRepository;

    public GetTodoByIdQuery(ITodoRepository todoRepository)
    {
        this.todoRepository = todoRepository;
    }

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
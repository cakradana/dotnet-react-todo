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

    public TodoResponse? Execute(long id)
    {
        var item = todoRepository.GetById(id);
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
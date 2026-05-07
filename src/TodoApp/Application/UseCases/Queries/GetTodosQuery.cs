using TodoApp.Application.Abstractions;
using TodoApp.Application.Contracts.Todos;
using TodoApp.Domain.Models;

namespace TodoApp.Application.UseCases.Queries;

public sealed class GetTodosQuery
{
    private readonly ITodoRepository todoRepository;

    public GetTodosQuery(ITodoRepository todoRepository)
    {
        this.todoRepository = todoRepository;
    }

    public async Task<List<TodoResponse>> ExecuteAsync()
    {
        var todos = await todoRepository.GetAllAsync();
        return todos.Select(MapToResponse).ToList();
    }

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
using TodoApp.Application.Abstractions;
using TodoApp.Application.Contracts.Todos;
using TodoApp.Domain.Models;

namespace TodoApp.Application.UseCases.Commands;

public sealed class CreateTodoCommand
{
    private readonly ITodoRepository todoRepository;

    public CreateTodoCommand(ITodoRepository todoRepository)
    {
        this.todoRepository = todoRepository;
    }

    public async Task<TodoResponse> ExecuteAsync(CreateTodoRequest request)
    {
        var todo = new TodoItem
        {
            Task = request.Task,
            Priority = request.Priority,
            CreatedAt = DateTimeOffset.UtcNow,
            Category = new TodoCategory
            {
                Name = request.Category.Name,
                Color = request.Category.Color
            }
        };

        var created = await todoRepository.AddAsync(todo);

        return new TodoResponse(
            created.Id,
            created.Task,
            created.IsCompleted,
            created.Priority,
            created.CreatedAt,
            new TodoCategoryDto(created.Category.Name, created.Category.Color));
    }
}
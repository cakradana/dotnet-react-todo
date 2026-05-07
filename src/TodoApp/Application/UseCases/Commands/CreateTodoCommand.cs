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

    public TodoResponse Execute(CreateTodoRequest request)
    {
        var created = todoRepository.Add(new TodoItem
        {
            Task = request.Task,
            Priority = request.Priority,
            Category = new TodoCategory
            {
                Name = request.Category.Name,
                Color = request.Category.Color
            }
        });

        return new TodoResponse(
            created.Id,
            created.Task,
            created.IsCompleted,
            created.Priority,
            created.CreatedAt,
            new TodoCategoryDto(created.Category.Name, created.Category.Color));
    }
}
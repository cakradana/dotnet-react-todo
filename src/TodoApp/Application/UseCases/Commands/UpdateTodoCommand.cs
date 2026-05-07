using TodoApp.Application.Abstractions;
using TodoApp.Application.Contracts.Todos;
using TodoApp.Domain.Models;

namespace TodoApp.Application.UseCases.Commands;

public sealed class UpdateTodoCommand
{
    private readonly ITodoRepository todoRepository;

    public UpdateTodoCommand(ITodoRepository todoRepository)
    {
        this.todoRepository = todoRepository;
    }

    public async Task<bool> ExecuteAsync(UpdateTodoRequest request)
    {
        return await todoRepository.UpdateAsync(new TodoItem
        {
            Id = request.Id,
            Task = request.Task,
            Priority = request.Priority,
            Category = new TodoCategory
            {
                Name = request.Category.Name,
                Color = request.Category.Color
            }
        });
    }
}
using TodoApp.Application.Abstractions;
using TodoApp.Application.Contracts.Todos;
using TodoApp.Domain.Models;

namespace TodoApp.Application.UseCases.Commands;

public sealed class UpdateTodoCommand
{
    private readonly ITodoRepository todoRepository;

    /// <summary>
    /// Initializes a new instance of <see cref="UpdateTodoCommand"/> with the specified todo repository.
    /// </summary>
    /// <param name="todoRepository">Repository used to persist and update todo items.</param>
    public UpdateTodoCommand(ITodoRepository todoRepository)
    {
        this.todoRepository = todoRepository;
    }

    /// <summary>
    /// Updates an existing todo item using values from the provided request.
    /// </summary>
    /// <param name="request">DTO containing the todo's updated values (Id, Task, Priority and Category).</param>
    /// <returns>`true` if the repository successfully updated the todo item, `false` otherwise.</returns>
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
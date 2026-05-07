using TodoApp.Application.Abstractions;
using TodoApp.Application.Contracts.Todos;
using TodoApp.Domain.Models;

namespace TodoApp.Application.UseCases.Commands;

public sealed class CreateTodoCommand
{
    private readonly ITodoRepository todoRepository;

    /// <summary>
    /// Initializes a new instance of <see cref="CreateTodoCommand"/> with the specified todo repository.
    /// </summary>
    /// <param name="todoRepository">Repository used to persist and retrieve todo items.</param>
    public CreateTodoCommand(ITodoRepository todoRepository)
    {
        this.todoRepository = todoRepository;
    }

    /// <summary>
    /// Creates a new todo item from the provided request, persists it, and returns a response representing the stored item.
    /// </summary>
    /// <param name="request">Details for the todo to create, including task text, priority, and category information.</param>
    /// <returns>A <see cref="TodoResponse"/> representing the persisted todo item, including its id, task, completion state, priority, creation timestamp, and category.</returns>
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
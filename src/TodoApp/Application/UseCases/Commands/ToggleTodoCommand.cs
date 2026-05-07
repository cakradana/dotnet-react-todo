using TodoApp.Application.Abstractions;

namespace TodoApp.Application.UseCases.Commands;

public sealed class ToggleTodoCommand
{
    private readonly ITodoRepository todoRepository;

    /// <summary>
    /// Initializes a new instance of <see cref="ToggleTodoCommand"/> with the specified repository.
    /// </summary>
    /// <param name="todoRepository">Repository used to toggle a todo item's completed state.</param>
    public ToggleTodoCommand(ITodoRepository todoRepository)
    {
        this.todoRepository = todoRepository;
    }

    /// <summary>
/// Toggles the completion status of the todo with the specified id.
/// </summary>
/// <param name="id">The identifier of the todo to toggle.</param>
/// <returns>`true` if the todo's completion status was toggled, `false` otherwise.</returns>
public async Task<bool> ExecuteAsync(long id) => await todoRepository.ToggleCompletedAsync(id);
}
using TodoApp.Application.Abstractions;

namespace TodoApp.Application.UseCases.Commands;

/// <summary>
/// Command to remove all completed todo items from the system.
/// </summary>
public sealed class ClearCompletedTodosCommand
{
    private readonly ITodoRepository repository;

    public ClearCompletedTodosCommand(ITodoRepository repository)
    {
    /// <summary>
    /// Executes the command to clear completed todos.
    /// </summary>
    /// <returns>The number of completed todos that were removed.</returns>
        this.repository = repository;
    }

    public async Task<int> ExecuteAsync()
    {
        return await repository.RemoveCompletedAsync();
    }
}
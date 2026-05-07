using TodoApp.Application.Abstractions;

namespace TodoApp.Application.UseCases.Commands;

/// <summary>
/// Command to remove all completed todo items from the system.
/// </summary>
public sealed class ClearCompletedTodosCommand
{
    private readonly ITodoRepository repository;

    /// <summary>
    /// Initializes a new <see cref="ClearCompletedTodosCommand"/> with the specified todo repository.
    /// </summary>
    /// <param name="repository">Repository used to remove completed todo items.</param>
    public ClearCompletedTodosCommand(ITodoRepository repository)
    {
    /// <summary>
    /// Executes the command to clear completed todos.
    /// </summary>
    /// <returns>The number of completed todos that were removed.</returns>
        this.repository = repository;
    }

    /// <summary>
    /// Clears all completed todo items from the repository and returns how many were removed.
    /// </summary>
    /// <returns>The number of completed todo items that were removed.</returns>
    public async Task<int> ExecuteAsync()
    {
        return await repository.RemoveCompletedAsync();
    }
}
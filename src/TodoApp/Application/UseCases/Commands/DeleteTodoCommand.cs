using TodoApp.Application.Abstractions;

namespace TodoApp.Application.UseCases.Commands;

public sealed class DeleteTodoCommand
{
    private readonly ITodoRepository todoRepository;

    /// <summary>
    /// Creates a DeleteTodoCommand using the provided todo repository.
    /// </summary>
    /// <param name="todoRepository">Repository used to perform persistence operations for todos.</param>
    public DeleteTodoCommand(ITodoRepository todoRepository)
    {
        this.todoRepository = todoRepository;
    }

    /// <summary>
    /// Delete the todo item with the specified identifier.
    /// </summary>
    /// <param name="id">The identifier of the todo item to delete.</param>
    /// <returns>`true` if the todo item was deleted, `false` otherwise.</returns>
    public async Task<bool> ExecuteAsync(long id)
    {
        return await todoRepository.DeleteAsync(id);
    }
}
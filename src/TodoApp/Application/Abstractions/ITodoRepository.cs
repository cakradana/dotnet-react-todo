using TodoApp.Domain.Models;

namespace TodoApp.Application.Abstractions;

public interface ITodoRepository
{
    /// <summary>
/// Retrieves all todo items from the repository.
/// </summary>
/// <returns>A list of all persisted <see cref="TodoItem"/> instances; empty list if none exist.</returns>
Task<List<TodoItem>> GetAllAsync();

    /// <summary>
/// Retrieves the todo item with the specified identifier.
/// </summary>
/// <param name="id">The identifier of the todo item to retrieve.</param>
/// <returns>The matching <see cref="TodoItem"/> if found; otherwise <c>null</c>.</returns>
Task<TodoItem?> GetByIdAsync(long id);

    /// <summary>
/// Adds a new todo item to the repository and returns the persisted entity.
/// </summary>
/// <param name="item">The todo item to add.</param>
/// <returns>The added <see cref="TodoItem"/> as stored by the repository.</returns>
Task<TodoItem> AddAsync(TodoItem item);

    /// <summary>
/// Updates an existing todo item in the repository.
/// </summary>
/// <param name="item">The todo item to update.</param>
/// <returns><c>true</c> if the item was updated; <c>false</c> otherwise.</returns>
Task<bool> UpdateAsync(TodoItem item);

    /// <summary>
/// Deletes the todo item with the specified identifier from the repository.
/// </summary>
/// <param name="id">The identifier of the todo item to delete.</param>
/// <returns>true if the item was deleted; false otherwise.</returns>
Task<bool> DeleteAsync(long id);

    /// <summary>
/// Toggles the completed state of the todo item with the specified id.
/// </summary>
/// <param name="id">The identifier of the todo item to toggle.</param>
/// <returns>`true` if the completion state was changed; `false` if the item was not found or the operation failed.</returns>
Task<bool> ToggleCompletedAsync(long id);

    /// <summary>
    /// Removes all completed todo items from the repository.
    /// </summary>
    /// <summary>
/// Removes all todo items marked as completed from the repository.
/// </summary>
/// <returns>The number of items removed.</returns>
    Task<int> RemoveCompletedAsync();
}
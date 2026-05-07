using TodoApp.Domain.Models;

namespace TodoApp.Application.Abstractions;

public interface ITodoRepository
{
    Task<List<TodoItem>> GetAllAsync();

    Task<TodoItem?> GetByIdAsync(long id);

    Task<TodoItem> AddAsync(TodoItem item);

    Task<bool> UpdateAsync(TodoItem item);

    Task<bool> DeleteAsync(long id);

    Task<bool> ToggleCompletedAsync(long id);

    /// <summary>
    /// Removes all completed todo items from the repository.
    /// </summary>
    /// <returns>The number of items removed.</returns>
    Task<int> RemoveCompletedAsync();
}
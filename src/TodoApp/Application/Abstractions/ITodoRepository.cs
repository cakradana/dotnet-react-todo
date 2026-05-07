using TodoApp.Domain.Models;

namespace TodoApp.Application.Abstractions;

public interface ITodoRepository
{
    List<TodoItem> GetAll();

    TodoItem? GetById(long id);

    TodoItem Add(TodoItem item);

    bool Update(TodoItem item);

    bool Delete(long id);

    bool ToggleCompleted(long id);

    /// <summary>
    /// Removes all completed todo items from the repository.
    /// </summary>
    /// <returns>The number of items removed.</returns>
    int RemoveCompleted();
}
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Abstractions;
using TodoApp.Domain.Models;
using TodoApp.Infrastructure.Data;

namespace TodoApp.Infrastructure.Persistence;

public class EfCoreTodoRepository : ITodoRepository
{
    private readonly TodoDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of <see cref="EfCoreTodoRepository"/> that uses the provided <see cref="TodoDbContext"/> for data access.
    /// </summary>
    /// <param name="dbContext">The EF Core context used to query and persist <see cref="TodoItem"/> entities.</param>
    public EfCoreTodoRepository(TodoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Retrieves all TodoItem entities from the persistent store.
    /// </summary>
    /// <returns>A list containing every TodoItem currently stored.</returns>
    public async Task<List<TodoItem>> GetAllAsync()
    {
        return await _dbContext.Todos.OrderBy(t => t.Id).ToListAsync();
    }

    /// <summary>
    /// Retrieves a todo item by its primary key.
    /// </summary>
    /// <param name="id">The primary key of the todo item to retrieve.</param>
    /// <returns>The matching <see cref="TodoItem"/>, or <c>null</c> if no item exists with the specified id.</returns>
    public async Task<TodoItem?> GetByIdAsync(long id)
    {
        return await _dbContext.Todos.FindAsync(id);
    }

    /// <summary>
    /// Adds the provided TodoItem to the data store and saves changes to the database.
    /// </summary>
    /// <param name="item">The TodoItem to add; after saving it may contain database-generated values (e.g., Id).</param>
    /// <returns>The same TodoItem instance, updated with any values assigned by the database.</returns>
    public async Task<TodoItem> AddAsync(TodoItem item)
    {
        _dbContext.Todos.Add(item);
        await _dbContext.SaveChangesAsync();
        return item;
    }

    /// <summary>
    /// Updates an existing todo item in the database using values from the provided item.
    /// </summary>
    /// <param name="item">A TodoItem whose Id identifies the entity to update and whose properties contain the new values.</param>
    /// <returns>`true` if an entity with the given Id was found and updated; `false` if no matching entity exists.</returns>
    public async Task<bool> UpdateAsync(TodoItem item)
    {
        var existing = await _dbContext.Todos.FindAsync(item.Id);
        if (existing is null)
        {
            return false;
        }

        existing.Task = item.Task;
        existing.Priority = item.Priority;
        // Since Category is an owned type, update its properties
        existing.Category.Name = item.Category.Name;
        existing.Category.Color = item.Category.Color;
        existing.IsCompleted = item.IsCompleted;
        
        await _dbContext.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Deletes the TodoItem with the specified id from the data store.
    /// </summary>
    /// <param name="id">The primary key of the TodoItem to delete.</param>
    /// <returns>`true` if an item was found and deleted, `false` otherwise.</returns>
    public async Task<bool> DeleteAsync(long id)
    {
        var item = await _dbContext.Todos.FindAsync(id);
        if (item is null)
        {
            return false;
        }

        _dbContext.Todos.Remove(item);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Toggle the completion state of the todo item with the given id and persist the change.
    /// </summary>
    /// <returns>`true` if an item with the specified id was found and its completion state was flipped, `false` otherwise.</returns>
    public async Task<bool> ToggleCompletedAsync(long id)
    {
        var item = await _dbContext.Todos.FindAsync(id);
        if (item is null)
        {
            return false;
        }

        item.IsCompleted = !item.IsCompleted;
        await _dbContext.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Removes all todo items marked as completed from the underlying data store.
    /// </summary>
    /// <returns>The number of todo items that were removed.</returns>
    public async Task<int> RemoveCompletedAsync()
    {
        var completedItems = await _dbContext.Todos.Where(t => t.IsCompleted).ToListAsync();
        var count = completedItems.Count;
        
        if (count > 0)
        {
            _dbContext.Todos.RemoveRange(completedItems);
            await _dbContext.SaveChangesAsync();
        }

        return count;
    }
}

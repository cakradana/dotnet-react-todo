using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Abstractions;
using TodoApp.Domain.Models;
using TodoApp.Infrastructure.Data;

namespace TodoApp.Infrastructure.Persistence;

public class EfCoreTodoRepository : ITodoRepository
{
    private readonly TodoDbContext _dbContext;

    public EfCoreTodoRepository(TodoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<TodoItem>> GetAllAsync()
    {
        return await _dbContext.Todos.OrderBy(t => t.Id).ToListAsync();
    }

    public async Task<TodoItem?> GetByIdAsync(long id)
    {
        return await _dbContext.Todos.FindAsync(id);
    }

    public async Task<TodoItem> AddAsync(TodoItem item)
    {
        _dbContext.Todos.Add(item);
        await _dbContext.SaveChangesAsync();
        return item;
    }

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

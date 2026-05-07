using System.Text.Json;
using TodoApp.Application.Abstractions;
using TodoApp.Domain.Models;

namespace TodoApp.Infrastructure.Persistence;

public sealed class TodoRepository : ITodoRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    private readonly string filePath;

    public TodoRepository(string filePath)
    {
        this.filePath = filePath;
    }

    public List<TodoItem> GetAll() => Load();

    public TodoItem? GetById(long id) => Load().FirstOrDefault(todo => todo.Id == id);

    public TodoItem Add(TodoItem item)
    {
        var todos = Load();
        item.Id = todos.Count == 0 ? 1 : todos.Max(todo => todo.Id) + 1;
        item.CreatedAt = item.CreatedAt == default ? DateTimeOffset.UtcNow : item.CreatedAt;

        todos.Add(item);
        Save(todos);

        return item;
    }

    public bool Update(TodoItem item)
    {
        var todos = Load();
        var index = todos.FindIndex(todo => todo.Id == item.Id);

        if (index < 0)
        {
            return false;
        }

        todos[index] = item;
        Save(todos);
        return true;
    }

    public bool Delete(long id)
    {
        var todos = Load();
        var removed = todos.RemoveAll(todo => todo.Id == id) > 0;

        if (removed)
        {
            Save(todos);
        }

        return removed;
    }

    public bool ToggleCompleted(long id)
    {
        var todos = Load();
        var todo = todos.FirstOrDefault(item => item.Id == id);

        if (todo is null)
        {
            return false;
        }

        todo.IsCompleted = !todo.IsCompleted;
        Save(todos);
        return true;
    }

    public int RemoveCompleted()
    {
        var todos = Load();
        var removedCount = todos.RemoveAll(todo => todo.IsCompleted);

        if (removedCount > 0)
        {
            Save(todos);
        }

        return removedCount;
    }

    private List<TodoItem> Load()
    {
        if (!File.Exists(filePath))
        {
            return [];
        }

        var json = File.ReadAllText(filePath);
        if (string.IsNullOrWhiteSpace(json))
        {
            return [];
        }

        return JsonSerializer.Deserialize<List<TodoItem>>(json, JsonOptions) ?? [];
    }

    private void Save(List<TodoItem> todos)
    {
        var json = JsonSerializer.Serialize(todos, JsonOptions);
        File.WriteAllText(filePath, json);
    }
}
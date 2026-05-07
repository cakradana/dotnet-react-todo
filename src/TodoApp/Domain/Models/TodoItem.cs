using System.Text.Json.Serialization;

namespace TodoApp.Domain.Models;

public sealed class TodoItem
{
    public long Id { get; set; }

    public string Task { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }

    public string Priority { get; set; } = "Medium";

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public TodoCategory Category { get; set; } = new();
}

public sealed class TodoCategory
{
    public string Name { get; set; } = string.Empty;

    public string Color { get; set; } = string.Empty;
}
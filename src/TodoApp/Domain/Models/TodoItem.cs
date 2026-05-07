using System.Text.Json.Serialization;

namespace TodoApp.Domain.Models;

public sealed class TodoItem
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("task")]
    public string Task { get; set; } = string.Empty;

    [JsonPropertyName("isCompleted")]
    public bool IsCompleted { get; set; }

    [JsonPropertyName("priority")]
    public string Priority { get; set; } = "Medium";

    [JsonPropertyName("createdAt")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    [JsonPropertyName("category")]
    public TodoCategory Category { get; set; } = new();
}

public sealed class TodoCategory
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("color")]
    public string Color { get; set; } = string.Empty;
}
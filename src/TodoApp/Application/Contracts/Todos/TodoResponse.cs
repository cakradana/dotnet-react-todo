namespace TodoApp.Application.Contracts.Todos;

public sealed record TodoResponse(
    long Id,
    string Task,
    bool IsCompleted,
    string Priority,
    DateTimeOffset CreatedAt,
    TodoCategoryDto Category);
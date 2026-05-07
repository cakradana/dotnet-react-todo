namespace TodoApp.Application.Contracts.Todos;

public sealed record CreateTodoRequest(string Task, string Priority, TodoCategoryDto Category);
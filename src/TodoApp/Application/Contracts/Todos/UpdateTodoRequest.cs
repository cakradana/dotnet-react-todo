namespace TodoApp.Application.Contracts.Todos;

public sealed record UpdateTodoRequest(long Id, string Task, string Priority, TodoCategoryDto Category) : ITodoRequest;
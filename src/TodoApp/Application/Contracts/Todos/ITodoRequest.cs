namespace TodoApp.Application.Contracts.Todos;

public interface ITodoRequest
{
    string Task { get; }
    string Priority { get; }
    TodoCategoryDto Category { get; }
}

using TodoApp.Application.Abstractions;

namespace TodoApp.Application.UseCases.Commands;

public sealed class ToggleTodoCommand
{
    private readonly ITodoRepository todoRepository;

    public ToggleTodoCommand(ITodoRepository todoRepository)
    {
        this.todoRepository = todoRepository;
    }

    public bool Execute(long id) => todoRepository.ToggleCompleted(id);
}
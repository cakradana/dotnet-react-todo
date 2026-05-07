using TodoApp.Application.Abstractions;

namespace TodoApp.Application.UseCases.Commands;

public sealed class ToggleTodoCommand
{
    private readonly ITodoRepository todoRepository;

    public ToggleTodoCommand(ITodoRepository todoRepository)
    {
        this.todoRepository = todoRepository;
    }

    public async Task<bool> ExecuteAsync(long id) => await todoRepository.ToggleCompletedAsync(id);
}
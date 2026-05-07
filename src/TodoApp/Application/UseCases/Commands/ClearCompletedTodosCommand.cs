using TodoApp.Application.Abstractions;

namespace TodoApp.Application.UseCases.Commands;

public sealed class ClearCompletedTodosCommand
{
    private readonly ITodoRepository repository;

    public ClearCompletedTodosCommand(ITodoRepository repository)
    {
        this.repository = repository;
    }

    public int Execute()
    {
        return repository.RemoveCompleted();
    }
}
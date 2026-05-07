using TodoApp.Application.Abstractions;

namespace TodoApp.Application.UseCases.Commands;

public sealed class DeleteTodoCommand
{
    private readonly ITodoRepository todoRepository;

    public DeleteTodoCommand(ITodoRepository todoRepository)
    {
        this.todoRepository = todoRepository;
    }

    public async Task<bool> ExecuteAsync(long id)
    {
        return await todoRepository.DeleteAsync(id);
    }
}
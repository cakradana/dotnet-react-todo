using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.UseCases.Commands;
using TodoApp.Application.UseCases.Queries;

namespace TodoApp.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<GetTodosQuery>();
        services.AddTransient<GetTodoByIdQuery>();
        services.AddTransient<CreateTodoCommand>();
        services.AddTransient<UpdateTodoCommand>();
        services.AddTransient<DeleteTodoCommand>();
        services.AddTransient<ToggleTodoCommand>();

        return services;
    }
}
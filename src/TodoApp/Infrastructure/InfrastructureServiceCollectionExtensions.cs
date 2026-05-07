using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.Abstractions;
using TodoApp.Infrastructure.Persistence;

namespace TodoApp.Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string todoFilePath)
    {
        services.AddSingleton<ITodoRepository>(_ => new TodoRepository(todoFilePath));

        return services;
    }
}
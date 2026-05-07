using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Abstractions;
using TodoApp.Infrastructure.Persistence;

namespace TodoApp.Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<Data.TodoDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<ITodoRepository, EfCoreTodoRepository>();

        return services;
    }
}
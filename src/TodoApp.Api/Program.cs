using Microsoft.EntityFrameworkCore;
using TodoApp.Application;
using TodoApp.Infrastructure;
using TodoApp.Infrastructure.Data;

using TodoApp.Api.ExceptionHandlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails(); // Menambahkan standar format error RFC 7807 (ProblemDetails)
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();

// Register TodoApp Layers
var dbPath = Path.Combine(builder.Environment.ContentRootPath, "todos.db");
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? $"Data Source={dbPath}";
builder.Services.AddInfrastructure(connectionString);
builder.Services.AddApplication();

var app = builder.Build();

// Menambahkan Global Exception Handler (sekarang logic ValidationException dipisah di `ValidationExceptionHandler`)
app.UseExceptionHandler();

// Apply Migrations
if (app.Environment.IsDevelopment() || args.Contains("--migrate"))
{
    await app.Services.InitializeDatabaseAsync();
    if (args.Contains("--migrate"))
    {
        return; // Exit after migration if invoked explicitly for migration
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }

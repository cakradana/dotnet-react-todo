using Microsoft.EntityFrameworkCore;
using TodoApp.Application;
using TodoApp.Infrastructure;
using TodoApp.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register TodoApp Layers
var dbPath = Path.Combine(builder.Environment.ContentRootPath, "todos.db");
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? $"Data Source={dbPath}";
builder.Services.AddInfrastructure(connectionString);
builder.Services.AddApplication();

var app = builder.Build();

// Apply Migrations
await app.Services.InitializeDatabaseAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.Contracts.Todos;
using TodoApp.Application;
using TodoApp.Application.UseCases.Commands;
using TodoApp.Application.UseCases.Queries;
using TodoApp.Infrastructure;

var services = new ServiceCollection();
var connectionString = "Data Source=todos.db";
services.AddInfrastructure(connectionString);
services.AddApplication();

using var serviceProvider = services.BuildServiceProvider();

var getTodosQuery = serviceProvider.GetRequiredService<GetTodosQuery>();
var getTodoByIdQuery = serviceProvider.GetRequiredService<GetTodoByIdQuery>();
var createTodoCommand = serviceProvider.GetRequiredService<CreateTodoCommand>();
var updateTodoCommand = serviceProvider.GetRequiredService<UpdateTodoCommand>();
var deleteTodoCommand = serviceProvider.GetRequiredService<DeleteTodoCommand>();
var toggleTodoCommand = serviceProvider.GetRequiredService<ToggleTodoCommand>();
var clearCompletedCommand = serviceProvider.GetRequiredService<ClearCompletedTodosCommand>();

while (true)
{
    Console.WriteLine();
    Console.WriteLine("Todo List App");
    Console.WriteLine("1. Lihat todo");
    Console.WriteLine("2. Tambah todo");
    Console.WriteLine("3. Ubah todo");
    Console.WriteLine("4. Hapus todo");
    Console.WriteLine("5. Toggle selesai");
    Console.WriteLine("6. Hapus semua todo selesai");
    Console.WriteLine("0. Keluar");
    Console.Write("Pilihan: ");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            await ShowTodos(getTodosQuery);
            break;
        case "2":
            await AddTodo(createTodoCommand);
            break;
        case "3":
            await EditTodo(getTodoByIdQuery, updateTodoCommand);
            break;
        case "4":
            await DeleteTodo(deleteTodoCommand);
            break;
        case "5":
            await ToggleTodo(toggleTodoCommand);
            break;
        case "6":
            await ClearCompleted(clearCompletedCommand);
            break;
        case "0":
            return;
        default:
            Console.WriteLine("Pilihan tidak valid.");
            break;
    }
}

static async Task ShowTodos(GetTodosQuery getTodosQuery)
{
    var todos = await getTodosQuery.ExecuteAsync();
    if (todos.Count == 0)
    {
        Console.WriteLine("Belum ada todo.");
        return;
    }

    foreach (var todo in todos.OrderBy(todo => todo.Id))
    {
        Console.WriteLine($"[{todo.Id}] {(todo.IsCompleted ? "[x]" : "[ ]")} {todo.Task} | {todo.Priority} | {todo.Category.Name} ({todo.Category.Color}) | {todo.CreatedAt:u}");
    }
}

static async Task AddTodo(CreateTodoCommand createTodoCommand)
{
    var task = Prompt("Task");
    var priority = Prompt("Priority (Low/Medium/High)", "Medium");
    var categoryName = Prompt("Category name", "General");
    var categoryColor = Prompt("Category color", "Gray");

    var created = await createTodoCommand.ExecuteAsync(new CreateTodoRequest(task, priority, new TodoCategoryDto(categoryName, categoryColor)));
    Console.WriteLine($"Todo ditambahkan dengan id {created.Id}.");
}

static async Task EditTodo(GetTodoByIdQuery getTodoByIdQuery, UpdateTodoCommand updateTodoCommand)
{
    if (!TryReadId(out var id))
    {
        return;
    }

    var existing = await getTodoByIdQuery.ExecuteAsync(id);
    if (existing is null)
    {
        Console.WriteLine("Todo tidak ditemukan.");
        return;
    }

    var request = new UpdateTodoRequest(
        existing.Id,
        Prompt("Task", existing.Task),
        Prompt("Priority (Low/Medium/High)", existing.Priority),
        new TodoCategoryDto(
            Prompt("Category name", existing.Category.Name),
            Prompt("Category color", existing.Category.Color)));

    if (await updateTodoCommand.ExecuteAsync(request))
    {
        Console.WriteLine("Todo berhasil diubah.");
    }
}

static async Task DeleteTodo(DeleteTodoCommand deleteTodoCommand)
{
    if (!TryReadId(out var id))
    {
        return;
    }

    if (await deleteTodoCommand.ExecuteAsync(id))
    {
        Console.WriteLine("Todo berhasil dihapus.");
        return;
    }

    Console.WriteLine("Todo tidak ditemukan.");
}

static async Task ToggleTodo(ToggleTodoCommand toggleTodoCommand)
{
    if (!TryReadId(out var id))
    {
        return;
    }

    if (await toggleTodoCommand.ExecuteAsync(id))
    {
        Console.WriteLine("Status todo berhasil diubah.");
        return;
    }

    Console.WriteLine("Todo tidak ditemukan.");
}

static async Task ClearCompleted(ClearCompletedTodosCommand clearCompletedCommand)
{
    Console.Write("Hapus semua todo yang sudah selesai? (y/n): ");
    var confirm = Console.ReadLine();
    if (!string.Equals(confirm, "y", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Dibatalkan.");
        return;
    }

    var count = await clearCompletedCommand.ExecuteAsync();
    if (count > 0)
    {
        Console.WriteLine($"{count} todo selesai berhasil dihapus.");
    }
    else
    {
        Console.WriteLine("Tidak ada todo yang sudah selesai untuk dihapus.");
    }
}

static bool TryReadId(out long id)
{
    Console.Write("Masukkan id: ");
    return long.TryParse(Console.ReadLine(), out id);
}

static string Prompt(string label, string? defaultValue = null)
{
    Console.Write(defaultValue is null ? $"{label}: " : $"{label} [{defaultValue}]: ");
    var input = Console.ReadLine();
    return string.IsNullOrWhiteSpace(input) ? defaultValue ?? string.Empty : input.Trim();
}
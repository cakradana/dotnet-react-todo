using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.Contracts.Todos;
using TodoApp.Application;
using TodoApp.Application.UseCases.Commands;
using TodoApp.Application.UseCases.Queries;
using TodoApp.Infrastructure;

var services = new ServiceCollection();
services.AddInfrastructure(AppPaths.GetTodoFilePath());
services.AddApplication();

using var serviceProvider = services.BuildServiceProvider();

var getTodosQuery = serviceProvider.GetRequiredService<GetTodosQuery>();
var getTodoByIdQuery = serviceProvider.GetRequiredService<GetTodoByIdQuery>();
var createTodoCommand = serviceProvider.GetRequiredService<CreateTodoCommand>();
var updateTodoCommand = serviceProvider.GetRequiredService<UpdateTodoCommand>();
var deleteTodoCommand = serviceProvider.GetRequiredService<DeleteTodoCommand>();
var toggleTodoCommand = serviceProvider.GetRequiredService<ToggleTodoCommand>();

while (true)
{
    Console.WriteLine();
    Console.WriteLine("Todo List App");
    Console.WriteLine("1. Lihat todo");
    Console.WriteLine("2. Tambah todo");
    Console.WriteLine("3. Ubah todo");
    Console.WriteLine("4. Hapus todo");
    Console.WriteLine("5. Toggle selesai");
    Console.WriteLine("0. Keluar");
    Console.Write("Pilihan: ");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            ShowTodos(getTodosQuery.Execute());
            break;
        case "2":
            AddTodo(createTodoCommand);
            break;
        case "3":
            EditTodo(getTodoByIdQuery, updateTodoCommand);
            break;
        case "4":
            DeleteTodo(deleteTodoCommand);
            break;
        case "5":
            ToggleTodo(toggleTodoCommand);
            break;
        case "0":
            return;
        default:
            Console.WriteLine("Pilihan tidak valid.");
            break;
    }
}

static void ShowTodos(List<TodoResponse> todos)
{
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

static void AddTodo(CreateTodoCommand createTodoCommand)
{
    var task = Prompt("Task");
    var priority = Prompt("Priority (Low/Medium/High)", "Medium");
    var categoryName = Prompt("Category name", "General");
    var categoryColor = Prompt("Category color", "Gray");

    var created = createTodoCommand.Execute(new CreateTodoRequest(task, priority, new TodoCategoryDto(categoryName, categoryColor)));
    Console.WriteLine($"Todo ditambahkan dengan id {created.Id}.");
}

static void EditTodo(GetTodoByIdQuery getTodoByIdQuery, UpdateTodoCommand updateTodoCommand)
{
    if (!TryReadId(out var id))
    {
        return;
    }

    var existing = getTodoByIdQuery.Execute(id);
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

    if (updateTodoCommand.Execute(request))
    {
        Console.WriteLine("Todo berhasil diubah.");
    }
}

static void DeleteTodo(DeleteTodoCommand deleteTodoCommand)
{
    if (!TryReadId(out var id))
    {
        return;
    }

    if (deleteTodoCommand.Execute(id))
    {
        Console.WriteLine("Todo berhasil dihapus.");
        return;
    }

    Console.WriteLine("Todo tidak ditemukan.");
}

static void ToggleTodo(ToggleTodoCommand toggleTodoCommand)
{
    if (!TryReadId(out var id))
    {
        return;
    }

    if (toggleTodoCommand.Execute(id))
    {
        Console.WriteLine("Status todo berhasil diubah.");
        return;
    }

    Console.WriteLine("Todo tidak ditemukan.");
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
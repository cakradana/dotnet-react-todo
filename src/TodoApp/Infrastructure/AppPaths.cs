namespace TodoApp.Infrastructure;

public static class AppPaths
{
    public static string GetTodoFilePath()
    {
        return Path.Combine(AppContext.BaseDirectory, "Data", "todo.json");
    }
}
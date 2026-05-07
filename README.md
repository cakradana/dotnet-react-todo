# .NET Clean Architecture Todo App

A step-by-step implementation of a Todo application demonstrating Clean Architecture principles in .NET. This project evolves progressively from a simple console application to a full-stack web application with an API, SQLite database, and React frontend.

## Project Evolution Plan

This project is built iteratively to emphasize understanding at each layer:

- ✅ **Step 1:** Console App Foundation (Basic CRUD operations)
- ✅ **Step 2:** Layering & Clean Architecture (Domain, Application, Infrastructure)
- ✅ **Step 3:** Web API (RESTful endpoints using `TodoApp.Api`)
- ⏳ **Step 4:** Database Integration (EF Core + SQLite)
- ⏳ **Step 5:** React Frontend UI

## Architecture

The application adheres to **Clean Architecture** with the following layers:

- **Domain Layer (`src/TodoApp/Domain`)**: Contains the core business rules, entities (`Todo`), and value objects (`TodoCategory`). This layer has zero external dependencies.
- **Application Layer (`src/TodoApp/Application`)**: Orchestrates business use cases (Commands & Queries). Defines the interfaces (Contracts/Abstractions) that the outer layers must implement.
- **Infrastructure Layer (`src/TodoApp/Infrastructure`)**: Implements data access. Currently uses file-based JSON persistence, but will be replaced by EF Core & SQLite.
- **Presentation Layer (`src/TodoApp/Presentation` & `src/TodoApp.Api`)**: The entry points of the application. Supports both a Console interface and a RESTful Web API.

### Project Structure (Target)

```text
dotnet-react-todo/
├── TodoApp.slnx
├── PLAN.md
└── src/
    ├── TodoApp.Api/                 # 1. PRESENTATION (WEB API)
    │   ├── Controllers/             # REST Endpoints
    │   ├── appsettings.json         # Configuration
    │   └── Program.cs               # API Bootstrap & DI Setup
    │
    ├── TodoApp/                     # PROJECT CORE BACKEND
    │   ├── Domain/                  # 2. DOMAIN LAYER
    │   │   ├── Entities/            # Core business models (Todo)
    │   │   └── ValueObjects/        # Static objects (TodoCategory)
    │   │
    │   ├── Application/             # 3. APPLICATION LAYER
    │   │   ├── Abstractions/        # Interfaces (ITodoRepository)
    │   │   ├── Contracts/           # DTOs (Request/Response)
    │   │   └── UseCases/            # Commands & Queries
    │   │
    │   ├── Infrastructure/          # 4. INFRASTRUCTURE LAYER
    │   │   ├── Data/                # EF Core DbContext (Upcoming)
    │   │   ├── Repositories/        # ITodoRepository Implementations
    │   │   └── DependencyInjection.cs
    │   │
    │   ├── Presentation/
    │   │   └── Console/             # Alternative Entry Point (Console App)
    │   │
    │   └── TodoApp.csproj
    │
    └── TodoApp.UI/                  # 5. FRONTEND (Upcoming React App)
        └── src/                     # React UI components and API clients
```

## Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download) (or the version specified in the project)
- A code editor like Visual Studio Code, Visual Studio, or JetBrains Rider.

## Getting Started

### 1. Running the Console Application

The console application provides an interactive CLI to manage your Todos.

```bash
cd src/TodoApp
dotnet run
```

### 2. Running the Web API

The Web API exposes RESTful endpoints for the Todo operations.

```bash
cd src/TodoApp.Api
dotnet run
```

By default, the API will be available at `http://localhost:5004` (or as configured in `launchSettings.json`).

**Available API Endpoints:**

- `GET /api/todos` - Get all todos
- `GET /api/todos/{id}` - Get a specific todo
- `POST /api/todos` - Create a new todo
- `PUT /api/todos/{id}` - Update an existing todo
- `PATCH /api/todos/{id}/toggle` - Toggle the completion status
- `DELETE /api/todos/{id}` - Delete a todo
- `DELETE /api/todos/completed` - Clear all completed todos

## Contributing / Progress

Please refer to `PLAN.md` to see the current status of the implementation and the exact roadmap.

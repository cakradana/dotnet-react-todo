---
description: "Instructions for work in src/TodoApp.Api"
applyTo: "src/TodoApp.Api/**"
---

- Follow RESTful API conventions when exposing endpoints.
- Map DTOs to business/application cases.
- Validate inputs correctly before handing off to Application layer use cases (e.g. `CreateTodoCommand`, `UpdateTodoCommand`).
- Keep controller actions thin, mostly wiring up to MediatR or direct UseCases.
- Rely on DI for dependencies, avoiding tight coupling to infrastructure where possible.

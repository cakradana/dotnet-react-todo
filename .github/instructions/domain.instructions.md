---
description: "Instructions for work in src/TodoApp/Domain"
applyTo: "src/TodoApp/Domain/**"
---

- Keep domain models pure and free of infrastructure concerns.
- Prefer small, stable domain entities and value objects.
- Do not reference Application, Infrastructure, or Presentation from Domain.
- Keep business rules that belong to the core model in Domain, not in UI or persistence.

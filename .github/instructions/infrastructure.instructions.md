---
description: "Instructions for work in src/TodoApp/Infrastructure"
applyTo: "src/TodoApp/Infrastructure/**"
---

- Keep persistence and external integrations here.
- Implement Application abstractions in this layer.
- Avoid putting business rules here unless they are strictly required by the persistence mechanism.
- Prefer clear, isolated adapters so future changes like SQLite or APIs are easy to swap in.

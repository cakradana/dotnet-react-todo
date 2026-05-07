---
description: "Instructions for work in src/TodoApp/Application"
applyTo: "src/TodoApp/Application/**"
---

- Keep use cases focused on application behavior, not UI or persistence details.
- Use contracts (DTOs) for input and output when the domain model should stay hidden.
- Keep commands and queries thin and explicit.
- Depend on abstractions from Application.Abstractions, not concrete infrastructure classes.
- If a change affects multiple layers, keep orchestration in the composition root.

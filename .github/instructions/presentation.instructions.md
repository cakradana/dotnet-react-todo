---
description: "Instructions for work in src/TodoApp/Presentation"
applyTo: "src/TodoApp/Presentation/**"
---

- Keep presentation code thin and focused on input/output.
- Do not place business logic here.
- Let the presentation layer call application use cases and handle formatting or parsing only.
- Keep startup wiring in the composition root and avoid scattering dependency creation.

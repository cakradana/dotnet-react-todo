# Repository Instructions

Follow these rules in every coding session for this repo, including new chats and different models:

1. Read [PLAN.md](PLAN.md) before coding.
2. Respect the current layered structure under `src/TodoApp`.
3. Remove unused code, files, and folders before adding new ones.
4. Update [PLAN.md](PLAN.md) whenever the plan, architecture, or repo structure changes materially.

Working rules:

- Keep domain code pure under `src/TodoApp/Domain`.
- Keep use cases and contracts under `src/TodoApp/Application`.
- Keep persistence and adapters under `src/TodoApp/Infrastructure`.
- Keep console/UI or other entrypoints under `src/TodoApp/Presentation`.
- Keep wiring in the composition root and keep business logic in use cases.
- Prefer small, reversible changes instead of broad rewrites.
- If you create a new layer or folder, remove the old unused one in the same pass when possible.

Current repo structure to respect:

- `src/TodoApp/Domain`
- `src/TodoApp/Application`
- `src/TodoApp/Infrastructure`
- `src/TodoApp/Presentation`

If you are unsure where to start, inspect [PLAN.md](PLAN.md) first and continue from the current status section.

# Todo App Plan

Kita akan tetap bertahap: console app dulu, lalu Web API, kemudian SQLite, dan React di paling akhir. Tujuannya supaya tiap lapisan dipahami dan bisa diuji sendiri tanpa loncat ke stack penuh terlalu cepat.

## Steps

1. Console app foundation.
   - Sudah dibuat di `src/TodoApp`.
   - Menjadi tempat belajar alur CRUD dasar tanpa database.

2. Layering and cleanup.
   - Domain model ada di `src/TodoApp/Domain`.
   - DTO/contract ada di `src/TodoApp/Application/Contracts`.
   - Use cases ada di `src/TodoApp/Application/UseCases`.
   - Infrastructure menangani persistence file-based di `src/TodoApp/Infrastructure`.
   - Startup wiring memakai DI lewat `AddApplication()` dan `AddInfrastructure()`.

3. Web API.
   - Buat project baru `src/TodoApp.Api`.
   - Pindahkan bootstrap ke `Program.cs` API.
   - Reuse use cases dan contracts yang sudah ada.
   - Expose CRUD endpoint untuk todo.

4. SQLite.
   - Ganti file-based persistence dengan EF Core + SQLite.
   - Pertahankan contract API agar tidak banyak berubah.

5. React UI.
   - Buat frontend terakhir sebagai consumer API.
   - Fokus ke daftar, tambah, edit, delete, dan toggle todo.

## Current status

- Console app sudah berjalan.
- DI sudah dipakai untuk Application dan Infrastructure.
- Use cases dan contracts sudah dipisah.
- Struktur repo sudah dibersihkan dari folder lama yang tidak dipakai.

## Relevant files

- `TodoApp.slnx`
- `src/TodoApp/TodoApp.csproj`
- `src/TodoApp/Presentation/Console/Program.cs`
- `src/TodoApp/Application/UseCases`
- `src/TodoApp/Application/Contracts`
- `src/TodoApp/Application/Abstractions`
- `src/TodoApp/Infrastructure`
- `src/TodoApp/Domain`

## Verification

1. `dotnet build` harus tetap sukses.
2. CRUD console harus bisa baca/tulis data awal.
3. Saat Web API dibuat, endpoint diuji dengan Swagger atau curl.
4. Saat SQLite masuk, migrasi dan restart app harus tetap konsisten.

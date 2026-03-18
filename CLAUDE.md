# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build entire solution
dotnet build Cover.slnx

# Run all tests
dotnet test Cover.slnx

# Run a single test project
dotnet test tests/Cover.Api.Tests/Cover.Api.Tests.csproj
dotnet test tests/Cover.Web.Tests/Cover.Web.Tests.csproj

# Run a specific test by name (partial match)
dotnet test --filter "BalanceServiceTests"
dotnet test --filter "EqualSplit_CorrectBalance"

# Run the API (requires PostgreSQL on localhost:5432)
dotnet run --project src/Cover.Api

# Run the Blazor frontend
dotnet run --project src/Cover.Web

# Run everything via Docker (recommended for full-stack dev)
docker compose up --build

# EF Core migrations
dotnet ef migrations add <MigrationName> --project src/Cover.Api
dotnet ef database update --project src/Cover.Api
```

**Dev URLs (local):** API at `http://localhost:5169`, Web at `http://localhost:5008`, Swagger at `http://localhost:5169/swagger`
**Docker URLs:** API at `http://localhost:5001`, Web at `http://localhost:5002`
**DB connection (local):** `Host=localhost;Database=splitclaude;Username=postgres;Password=postgres`

## Architecture

Cover is a two-person expense-splitting app built on .NET 10. Three projects share code via `Cover.Shared`:

- **`Cover.Api`** — ASP.NET Core REST API with EF Core + PostgreSQL. Auto-migrates on startup. Controllers are thin; all logic lives in scoped services (`UserService`, `ExpenseService`, `BalanceService`). `ExceptionHandlingMiddleware` maps `KeyNotFoundException` → 404, everything else → 500.
- **`Cover.Web`** — Blazor WebAssembly SPA. `App.razor` checks setup status on load and redirects to `/setup` if the two users haven't been created yet. All API calls go through `IApiClient`/`ApiClient`. In Docker, nginx proxies `/api/*` to the API container.
- **`Cover.Shared`** — Record-type DTOs only. Referenced by both API and Web. No logic here.

### Key domain concepts

**Amount is stored as `long` (integer cents/ISK øre)** — not decimal. Format for display with `is-IS` culture (`"N0"` + `" kr."`).

**SplitType** drives balance calculation:
- `Equal` — payer is owed 50% by the other person
- `FullOther` — payer is owed 100% by the other person
- `NotShared` — personal expense, no effect on balance

**Setup flow** — the app requires exactly 2 users (`POST /api/setup`). `IsSetupCompleteAsync()` checks `Users.Count >= 2`. Setup can only be done once.

### Testing

- **API tests** (`Cover.Api.Tests`) use xUnit + EF Core in-memory database. Tests instantiate services directly with a real `AppDbContext` backed by in-memory storage.
- **Web tests** (`Cover.Web.Tests`) use bUnit + NSubstitute. `IApiClient` is mocked; components are rendered and asserted against HTML output.

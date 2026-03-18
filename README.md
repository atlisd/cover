# Cover

A simple expense-splitting app for two people. Track shared expenses, see who paid what, and know exactly who owes whom at any time.

## Stack

- **Backend:** ASP.NET Core Web API (.NET 10), Entity Framework Core, PostgreSQL
- **Frontend:** Blazor WebAssembly
- **Containerization:** Docker + Docker Compose

## Getting started

### With Docker (recommended)

```bash
docker compose up --build
```

- Web: http://localhost:5002
- API: http://localhost:5001
- Swagger: http://localhost:5001/swagger

### Local development

Requires .NET 10 SDK and a PostgreSQL instance at `localhost:5432` with database `splitclaude`, user `postgres`, password `postgres`.

```bash
# Run the API
dotnet run --project src/Cover.Api

# Run the frontend (separate terminal)
dotnet run --project src/Cover.Web
```

- API: http://localhost:5169
- Web: http://localhost:5008

## How it works

On first launch you enter two names. From then on you can log expenses, choosing who paid and how the expense is split:

| Split type | Effect on balance |
|---|---|
| **Equal** | Payer is owed 50% by the other person |
| **Owed by other** | Payer is owed 100% by the other person |
| **Personal** | No effect on balance |

The dashboard shows the current balance and recent expenses. The history page lets you filter and paginate through all expenses.

## License

[MIT](LICENSE)

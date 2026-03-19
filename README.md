# Cover

A simple expense-splitting app for two people. Track shared expenses, see who paid what, and know exactly who owes whom at any time.

## Stack

- **Backend:** ASP.NET Core Web API (.NET 10), Entity Framework Core, PostgreSQL
- **Frontend:** Blazor WebAssembly
- **Containerization:** Docker + Docker Compose

## Getting started

### With Docker (recommended)

1. Copy the example environment file and set a strong database password:

```bash
cp .env.example .env
```

Edit `.env` and change `POSTGRES_PASSWORD` to a strong password. For local development the defaults are fine.

2. Start the stack:

```bash
docker compose up --build
```

- Web: http://localhost:5002
- API: http://localhost:5001
- Swagger: http://localhost:5001/swagger

### Local development

Requires .NET 10 SDK and a PostgreSQL instance at `localhost:5432` with database `splitclaude`, user `postgres`, password `postgres` (configured in `appsettings.Development.json`).

```bash
# Run the API
dotnet run --project src/Cover.Api

# Run the frontend (separate terminal)
dotnet run --project src/Cover.Web
```

- API: http://localhost:5169
- Web: http://localhost:5008

> **Note:** Running outside Docker means the frontend and API are on different ports. Authentication uses httpOnly cookies which work automatically in the Docker setup (same-origin via nginx). For bare-metal local dev, use the Docker setup for the most reliable experience.

## Production deployment

The app is designed to run behind a reverse proxy (e.g. Nginx Proxy Manager) that handles SSL termination.

1. Set a strong `POSTGRES_PASSWORD` in your `.env` file
2. Run `docker compose up -d`
3. Point your reverse proxy to the `web` container on port 5002 (or adjust the published port in `docker-compose.yml`)
4. Configure SSL in your reverse proxy (e.g. Let's Encrypt via Nginx Proxy Manager)

The API sets `Secure` cookies in production, so HTTPS is required.

## How it works

On first launch you enter two names. From then on you can log expenses, choosing who paid and how the expense is split:

| Split type | Effect on balance |
|---|---|
| **Equal** | Payer is owed 50% by the other person |
| **Owed by other** | Payer is owed 100% by the other person |

The dashboard shows the current balance and recent expenses. The history page lets you filter and paginate through all expenses.

## License

[MIT](LICENSE)

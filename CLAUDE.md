# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & Run Commands

```bash
# Build the solution
dotnet build

# Run the API directly (without Aspire orchestration)
dotnet run --project CodeLab.FusionCache.Api

# Run via Aspire (recommended — starts PostgreSQL container + dashboard)
dotnet run --project CodeLab.FusionCache.AppHost

# EF Core migrations
dotnet ef migrations add <MigrationName> --project CodeLab.FusionCache.Api
dotnet ef database update --project CodeLab.FusionCache.Api
```

The API is available at `http://localhost:5174` (HTTP) or `https://localhost:7040` (HTTPS). The Aspire dashboard is at `http://localhost:15267`.

## Architecture

Three-project .NET 10 solution using **Aspire** for local orchestration:

- **`CodeLab.FusionCache.Api`** — Main REST API. Layered: Minimal API endpoints → `TodoService` → `TodoRepository` → EF Core (`AppDbContext`) → PostgreSQL.
- **`CodeLab.FusionCache.AppHost`** — Aspire orchestrator. Defines the distributed app topology: spins up PostgreSQL + PgAdmin containers and wires service references.
- **`CodeLab.FusionCache.ServiceDefaults`** — Shared cross-cutting concerns applied to all services: OpenTelemetry (traces/metrics/logs via OTLP), HTTP resilience handlers, service discovery, and health check endpoints (`/health`, `/alive`).

## Caching Pattern

**FusionCache** (`ZiggyCreatures.FusionCache`) is the central concern of this project. It provides a two-level cache (L1 memory + L2 distributed) with a cache-aside pattern.

- Default entry options are configured in `Program.cs` (5-minute duration for both L1 and L2).
- Cache keys are defined in the service layer (e.g., `"TodosList"` in `TodoService`).
- `TodoService.GetAllAsync()` demonstrates the standard usage: `GetOrSetAsync` with a factory that falls back to the repository.

## Key Dependencies

| Package | Purpose |
|---|---|
| `ZiggyCreatures.FusionCache` 2.6.0 | Hybrid L1/L2 caching |
| `Npgsql.EntityFrameworkCore.PostgreSQL` 10.x | PostgreSQL via EF Core |
| `FluentValidation.AspNetCore` 12.x | Input validation |
| `Scalar.AspNetCore` | OpenAPI UI (replaces Swagger) |
| `Aspire.Hosting.*` 13.x | Local dev orchestration |
| `OpenTelemetry.*` 1.x | Observability |

## Data Model

`Todo` entity: `Id` (UUIDv7), `Title` (max 50), `Description` (max 255, optional), `IsCompleted`, `CreatedAt`, `UpdatedAt`. EF Core configuration is in `Data/Configs/TodoConfiguration.cs` using Fluent API.

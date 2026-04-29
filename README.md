# IceCoffee.AspNetCore

A lightweight, opinionated ASP.NET Core library that provides a `StartupBase` class and a set of utilities to bootstrap web API applications with sensible defaults — authentication, Swagger (NSwag), CORS, Serilog structured logging, Mapster object mapping, and response caching — all driven by configuration flags.

---

## Target Frameworks

| Framework | Version |
|-----------|---------|
| .NET      | 8.0, 10.0 |

---

## Features

| Feature | Description |
|---------|-------------|
| **`StartupBase`** | Opinionated base class for `ConfigureServices` / `Configure`. Derive and override only what you need. |
| **Global authentication** | Every controller endpoint requires an authenticated user by default (`AuthorizeFilter`). |
| **HTTP Basic Auth** | Optional `idunno.Authentication.Basic`-powered Basic Auth, fully configured from `appsettings.json`. |
| **Swagger / OpenAPI** | NSwag document generation with Basic Auth security definition, controller-summary-based tag descriptions, and `PathBase`-aware server URL. |
| **CORS** | Configurable allowed origins; falls back to `AllowAnyOrigin` when none are specified. |
| **Serilog** | Structured logging with `ReadFrom.Configuration`, optional per-request HTTP logging, and an `ArchiveHooks` helper for automatic log file compression and 180-day retention. |
| **Mapster** | FastExpressionCompiler-powered global type adapter with built-in `string?` ↔ `JsonNode?` mappings. |
| **Response caching** | Pre-configured with case-insensitive paths, 64 MB max body, and 200 MB total size limit. |
| **`DateTimeConverter`** | Forces all `DateTime` values in JSON responses to the `yyyy-MM-ddTHH:mm:ss.fffZ` format. |
| **Forwarded headers** | Trusts all forwarded headers and clears known-network/proxy restrictions for reverse-proxy deployments. |
| **Static files** | Serves `wwwroot` when present; applies `no-cache` headers to `index.html` to prevent stale SPA shells. |
| **`HttpContextExtension`** | Extension methods for remote IP, request culture, and absolute URL reconstruction. |

---

## Installation

```shell
dotnet add package IceCoffee.AspNetCore
```

---

## Quick Start

### 1. Derive from `StartupBase`

```csharp
using IceCoffee.AspNetCore;

public class Startup : StartupBase
{
    public Startup(WebApplicationBuilder builder) : base(builder) { }

    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services); // apply all defaults

        // Register your own services here
        services.AddScoped<IMyService, MyService>();
    }

    public override void Configure(WebApplication app, IServiceProvider services)
    {
        // Add your own middleware / endpoints here
    }
}
```

### 2. Wire it up in `Program.cs`

```csharp
var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder);
startup.ConfigureServices(builder.Services);

var app = builder.Build();
startup.Configure(app, app.Services);

app.Run();
```

---

## Configuration Reference

All flags are read from `appsettings.json` (or any registered `IConfiguration` provider).

```jsonc
{
  // Enable Serilog per-request HTTP logging
  "EnableRequestLog": true,

  // Mount the application under a sub-path (e.g. "/api")
  "PathBase": "",

  // Expose NSwag Swagger UI at /swagger
  "EnableSwagger": true,

  // Enable the CORS policy
  "EnableCors": true,

  // Allowed origins for CORS; omit or leave empty to allow any origin
  "AllowedOrigins": [ "https://example.com" ],

  // HTTP Basic Authentication
  "BasicAuthOptions": {
    "Enabled": false,
    "Realm": "Basic Authentication",
    "UserName": "<set via secrets / env var>",
    "Password": "<set via secrets / env var>"
  },

  // Serilog configuration (standard Serilog.AspNetCore schema)
  "Serilog": {
    "MinimumLevel": { "Default": "Information" },
    "WriteTo": [
      {
        "Name": "File",
        "Args": {
          "path": "logs/log-.txt",
          "rollingInterval": "Day",
          "hooks": "IceCoffee.AspNetCore.SerilogHooks::ArchiveHooks, IceCoffee.AspNetCore"
        }
      }
    ]
  }
}
```

> **Security note:** Never store `BasicAuthOptions:UserName` or `BasicAuthOptions:Password` in source-controlled `appsettings.json`. Use [.NET user-secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) in development and environment variables / a secrets manager in production.

---

## Utilities

### `SerilogHooks.ArchiveHooks`

A pre-built `ArchiveHooks` instance (180-day retention, smallest compression) ready to attach to a Serilog rolling-file sink.

### `DateTimeConverter`

Registered globally in `ConfigureServices`. Serialises `DateTime` as `yyyy-MM-ddTHH:mm:ss.fffZ`; deserialises any ISO 8601 string.

### `HttpContextExtension`

| Method | Description |
|--------|-------------|
| `GetRemoteIpAddress()` | Client IP after forwarded-headers resolution. |
| `GetCulture()` | BCP 47 culture tag resolved by the localisation middleware. |
| `GetCurrentUri()` | Fully qualified absolute URL of the current request. |

---

## License

Copyright © 2026 IceCoffee. See [LICENSE](LICENSE) for details.

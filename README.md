# IceCoffee.AspNetCore

A lightweight, opinionated ASP.NET Core library that provides a `StartupBase` class and a set of utilities to bootstrap web API applications with sensible defaults — authentication, Swagger (NSwag), CORS, Serilog structured logging, Mapster object mapping, and response caching — all driven by configuration flags.

| Package | NuGet Stable | Downloads |
| ------- | ------------ | --------- |
| [IceCoffee.AspNetCore](https://www.nuget.org/packages/IceCoffee.AspNetCore/) | [![IceCoffee.AspNetCore](https://img.shields.io/nuget/v/IceCoffee.AspNetCore.svg)](https://www.nuget.org/packages/IceCoffee.AspNetCore/) | [![IceCoffee.AspNetCore](https://img.shields.io/nuget/dt/IceCoffee.AspNetCore.svg)](https://www.nuget.org/packages/IceCoffee.AspNetCore/) |

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0%20%7C%2010.0-512BD4)](https://dotnet.microsoft.com/)

---

## Features

| Feature | Description |
|---------|-------------|
| **`StartupBase`** | Opinionated base class for `ConfigureServices` / `Configure`. Derive and override only what you need. |
| **Global authentication** | Every controller endpoint requires an authenticated user when a scheme is registered (`AuthorizeFilter` is only added when `ConfigureAuthentication` returns `true`). |
| **HTTP Basic Auth** | Optional `idunno.Authentication.Basic`-powered Basic Auth, fully configured from `appsettings.json`. |
| **Swagger / OpenAPI** | NSwag document generation with pluggable security definition (`ConfigureSwaggerSecurity`), controller-summary-based tag descriptions, and `PathBase`-aware server URL. |
| **CORS** | Configurable allowed origins; falls back to `AllowAnyOrigin` when none are specified. |
| **Serilog** | Structured logging with `ReadFrom.Configuration`, optional per-request HTTP logging, and an `ArchiveHooks` helper for automatic log file compression and 180-day retention. |
| **Mapster** | FastExpressionCompiler-powered global type adapter with built-in `string?` ↔ `JsonNode?` mappings. |
| **Response caching** | Service registered with case-insensitive paths, 64 MB max body, and 200 MB total size limit. Middleware is not added to the pipeline by default — call `app.UseResponseCaching()` in your `Configure` override when needed. |
| **Exception handling** | Built-in `UseExceptionHandler` delegate returns RFC 7807 `ProblemDetails` JSON for all unhandled exceptions. The library also ships `CustomExceptionHandler` — an `IExceptionHandler` implementation that adds structured logging with trace IDs. Register it with `services.AddExceptionHandler<CustomExceptionHandler>()` when per-exception log entries are needed. Exception detail is exposed only in the `Development` environment. |
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
        base.Configure(app, services); // apply default middleware pipeline

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

## Extensibility

`StartupBase` exposes three `protected virtual` hooks for the most common customisation points:

| Method | Purpose | Default behaviour |
|--------|---------|-------------------|
| `ConfigureAuthentication(services)` | Register any authentication scheme. Return `true` to activate the global `AuthorizeFilter` and auth middleware; `false` to disable authentication entirely. | Reads `BasicAuthOptions` from config and registers HTTP Basic Auth when `Enabled = true`. |
| `ConfigureSwaggerSecurity(config)` | Add the Swagger/OpenAPI security definition and operation processor that matches the registered scheme. Called automatically when `ConfigureAuthentication` returned `true`. | Registers a Basic Auth security scheme. |
| `ConfigureServices(services)` | Override to add extra DI registrations after calling `base.ConfigureServices`. | Registers all built-in services. |
| `Configure(app, services)` | Override to append middleware or endpoints after calling `base.Configure`. | Builds the standard middleware pipeline. |

### Example — replacing Basic Auth with JWT Bearer

```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;
using NSwag;
using NSwag.Generation.AspNetCore;
using NSwag.Generation.Processors.Security;

public class Startup : StartupBase
{
    public Startup(WebApplicationBuilder builder) : base(builder) { }

    protected override bool ConfigureAuthentication(IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration["Jwt:Authority"];
                    options.Audience  = Configuration["Jwt:Audience"];
                });
        // AddAuthorization() is handled by the base class automatically.
        return true;
    }

    protected override void ConfigureSwaggerSecurity(AspNetCoreOpenApiDocumentGeneratorSettings config)
    {
        config.AddSecurity("Bearer", new OpenApiSecurityScheme
        {
            Type         = OpenApiSecuritySchemeType.Http,
            Scheme       = "bearer",
            BearerFormat = "JWT"
        });
        config.OperationProcessors.Add(new OperationSecurityScopeProcessor("Bearer"));
    }
}
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

> **Security note:** Never store `BasicAuthOptions:UserName` or `BasicAuthOptions:Password` in source-controlled `appsettings.json`. Use [.NET user-secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) in development and environment variables or a secrets manager in production.

> **Exception detail note:** `feature?.Error.ToString()` (full stack trace) is written to `ProblemDetails.detail` only when the hosting environment is `Development`. In all other environments `detail` is omitted to avoid information disclosure.

---

## Utilities

### `CustomExceptionHandler`

An `IExceptionHandler` implementation that logs every unhandled exception (including the distributed trace ID) and writes an RFC 7807 `ProblemDetails` 500 response. Register it alongside the built-in handler for structured per-exception log entries:

```csharp
// In your Startup.ConfigureServices override:
services.AddExceptionHandler<CustomExceptionHandler>();
```

The `detail` field is populated with the full `Exception.ToString()` output only in the `Development` environment.

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

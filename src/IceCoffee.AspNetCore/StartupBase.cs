using FastExpressionCompiler;
using IceCoffee.AspNetCore.JsonConverters;
using IceCoffee.AspNetCore.Options;
using idunno.Authentication.Basic;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NSwag;
using Serilog;
using System.Security.Claims;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace IceCoffee.AspNetCore
{
    /// <summary>
    /// Base startup class that encapsulates the standard service-registration and middleware-pipeline
    /// configuration for ASP.NET Core web API applications. Derive from this class to apply
    /// project-specific overrides while inheriting the opinionated defaults (authentication,
    /// Swagger via NSwag, CORS, Serilog, Mapster, response caching, etc.).
    /// </summary>
    public class StartupBase
    {
        /// <summary>
        /// Tracks whether any authentication scheme was registered during <see cref="ConfigureAuthentication"/>.
        /// Used to gate the global <see cref="AuthorizeFilter"/> and the authentication/authorisation
        /// middleware so that neither is added when no scheme is configured, avoiding the
        /// <see cref="InvalidOperationException"/> thrown by ASP.NET Core when a challenge is issued
        /// without a default scheme.
        /// </summary>
        private bool _authenticationConfigured;
        
        /// <summary>
        /// The application configuration sourced from <c>appsettings.json</c>, environment variables,
        /// and other registered providers. Used throughout service registration to read feature flags
        /// such as <c>EnableSwagger</c>, <c>EnableCors</c>, and <c>BasicAuthOptions</c>.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// The hosting environment, used to apply environment-specific behaviour such as
        /// exposing exception details only in development or conditionally serving static files.
        /// </summary>
        public IWebHostEnvironment Environment { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="StartupBase"/> by capturing the configuration
        /// and environment from the supplied builder before <c>Build()</c> is called.
        /// </summary>
        /// <param name="builder">The <see cref="WebApplicationBuilder"/> created in <c>Program.cs</c>.</param>
        public StartupBase(WebApplicationBuilder builder)
        {
            Configuration = builder.Configuration;
            Environment = builder.Environment;
        }

        /// <summary>
        /// Registers the authentication scheme(s) for this application. Override this method in a
        /// derived class to replace the default HTTP Basic Auth with any other scheme (OAuth2, OIDC,
        /// JWT Bearer, etc.). The return value controls whether the global <see cref="AuthorizeFilter"/>
        /// and the <c>UseAuthentication</c> / <c>UseAuthorization</c> middleware are activated;
        /// return <see langword="false"/> only when the application intentionally has no authentication.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to register authentication services into.</param>
        /// <returns>
        /// <see langword="true"/> if at least one authentication scheme was registered;
        /// <see langword="false"/> if authentication should be skipped entirely.
        /// </returns>
        protected virtual bool ConfigureAuthentication(IServiceCollection services)
        {
            var basicAuthOptions = Configuration.GetSection("BasicAuthOptions").Get<BasicAuthOptions>();
            if (basicAuthOptions == null || !basicAuthOptions.Enabled)
            {
                return false;
            }

            services.AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
                .AddBasic(options =>
                {
                    options.Realm = basicAuthOptions.Realm;
                    options.AllowInsecureProtocol = true;
                    options.Events = new BasicAuthenticationEvents
                    {
                        OnValidateCredentials = context =>
                        {
                            if (context.Username == basicAuthOptions.UserName && context.Password == basicAuthOptions.Password)
                            {
                                var claims = new[]
                                {
                                    new Claim(ClaimTypes.NameIdentifier, context.Username, ClaimValueTypes.String, context.Options.ClaimsIssuer),
                                    new Claim(ClaimTypes.Name, context.Username, ClaimValueTypes.String, context.Options.ClaimsIssuer)
                                };

                                context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name));
                                context.Success();
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            return true;
        }

        /// <summary>
        /// Adds the authentication-related security definition and operation processor to the NSwag
        /// document configuration. Called only when <see cref="ConfigureAuthentication"/> returned
        /// <see langword="true"/>. Override this method in a derived class to replace the default
        /// Basic Auth Swagger security definition with one that matches the scheme registered in
        /// <see cref="ConfigureAuthentication"/> (e.g. Bearer/JWT, OAuth2, API key).
        /// </summary>
        /// <param name="config">
        /// The NSwag <see cref="NSwag.Generation.AspNetCore.AspNetCoreOpenApiDocumentGeneratorSettings"/> being configured.
        /// </param>
        protected virtual void ConfigureSwaggerSecurity(NSwag.Generation.AspNetCore.AspNetCoreOpenApiDocumentGeneratorSettings config)
        {
            config.AddSecurity(BasicAuthenticationDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.Basic,
                Scheme = BasicAuthenticationDefaults.AuthenticationScheme
            });

            config.OperationProcessors.Add(
                new NSwag.Generation.Processors.Security.OperationSecurityScopeProcessor(
                    BasicAuthenticationDefaults.AuthenticationScheme));
        }


        /// <summary>
        /// Registers all framework and application services into the DI container.
        /// Calls <see cref="ConfigureAuthentication"/> first and uses its result to conditionally
        /// apply the global <see cref="AuthorizeFilter"/>, Swagger security definitions, and the
        /// authentication/authorisation middleware — preventing the
        /// <see cref="InvalidOperationException"/> that ASP.NET Core throws when a challenge is
        /// issued without a registered default scheme.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to register services into.</param>
        public virtual void ConfigureServices(IServiceCollection services)
        {
            // Delegate authentication setup to the virtual method so subclasses can swap in
            // any scheme (OAuth2, OIDC, JWT, etc.) without touching the rest of the pipeline.
            _authenticationConfigured = ConfigureAuthentication(services);
            if (_authenticationConfigured)
            {
                services.AddAuthorization();
            }

            // Add services to the container.
            services.AddControllers(config =>
            {
                // Only enforce global authentication when a scheme has actually been registered;
                // otherwise ASP.NET Core throws InvalidOperationException on the first challenge.
                if (_authenticationConfigured)
                {
                    var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                    config.Filters.Add(new AuthorizeFilter(policy));
                }
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
            });

            services.AddMemoryCache();
            services.AddProblemDetails();
            services.AddExceptionHandler<CustomExceptionHandler>();

            services.AddSingleton<FileExtensionContentTypeProvider>();

            services.Configure((FormOptions option) =>
            {
                option.MultipartBodyLengthLimit = int.MaxValue;
            });

            #region Serilog

            services.AddSerilog((services, lc) => lc
                    .ReadFrom.Configuration(Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext());

            #endregion

            #region Swagger

            if (Configuration.GetValue<bool>("EnableSwagger"))
            {
                // Register the Swagger services
                services.AddOpenApiDocument(config =>
                {
                    config.PostProcess = document =>
                    {
                        document.Info.Version = "v1";
                        document.Info.Title = "ASP.NET Core WebApi Documentation";
                        document.Info.Description = "";
                    };

                    // You can set it to load from an annotation file, but the loaded content can be overwritten by the OpenApiTagAttribute attribute.
                    config.UseControllerSummaryAsTagDescription = true;

                    if (_authenticationConfigured)
                    {
                        ConfigureSwaggerSecurity(config);
                    }
                });
            }

            #endregion

            #region ForwardedHeader

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.All;
#if NET10_0_OR_GREATER
                options.KnownIPNetworks.Clear();
#else
                options.KnownNetworks.Clear();
#endif
                options.KnownProxies.Clear();
            });

            #endregion

            #region Cors

            if (Configuration.GetValue<bool>("EnableCors"))
            {
                var allowedOrigins = Configuration.GetValue<string[]>("AllowedOrigins");

                services.AddCors(options =>
                {
                    options.AddPolicy("Cors", builder =>
                    {
                        if (allowedOrigins == null || allowedOrigins.Length == 0)
                        {
                            builder.AllowAnyOrigin();
                        }
                        else
                        {
                            builder.WithOrigins(allowedOrigins);
                        }
                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();
                    });
                });
            }

            #endregion

            services.AddResponseCaching(options =>
            {
                options.UseCaseSensitivePaths = false;
                options.MaximumBodySize = 64 * 1024 * 1024; // 64MB
                options.SizeLimit = 200 * 1024 * 1024; // 200MB
            });
        }

        /// <summary>
        /// Builds the HTTP request pipeline by adding middleware in the correct order.
        /// Also bootstraps Mapster's global type-adapter settings and conditionally activates
        /// request logging, Swagger UI, static files, CORS, and authentication middleware
        /// according to configuration flags.
        /// </summary>
        /// <param name="app">The <see cref="WebApplication"/> that owns the middleware pipeline.</param>
        /// <param name="services">
        /// The resolved <see cref="IServiceProvider"/>, used to retrieve configured options
        /// (e.g. <see cref="JsonOptions"/>) that must be reused during Mapster setup.
        /// </param>
        public virtual void Configure(WebApplication app, IServiceProvider services)
        {
            var jsonOptions = services.GetRequiredService<IOptions<JsonOptions>>().Value.JsonSerializerOptions;

            TypeAdapterConfig.GlobalSettings.Compiler = exp => exp.CompileFast();
            TypeAdapterConfig.GlobalSettings.NewConfig<string?, JsonNode?>().MapWith(src => src == null ? null : JsonNode.Parse(src, null, default));
            TypeAdapterConfig.GlobalSettings.NewConfig<JsonNode?, string?>().MapWith(src => src == null ? null : src.ToJsonString(jsonOptions));
            // TypeAdapterConfig.GlobalSettings.Default.NameMatchingStrategy(NameMatchingStrategy.IgnoreCase);

            bool enableRequestLog = Configuration.GetValue<bool>("EnableRequestLog");
            if (enableRequestLog)
            {
                app.UseSerilogRequestLogging();
            }

            app.UseExceptionHandler();
            app.UseStatusCodePages();
            app.UseForwardedHeaders();

            string? pathBase = Configuration.GetValue<string>("PathBase");
            if (string.IsNullOrEmpty(pathBase) == false)
            {
                app.UsePathBase(pathBase);
            }

            if (Configuration.GetValue<bool>("EnableSwagger"))
            {
                // Register the Swagger endpoint and the Swagger UI middlewares
                app.UseOpenApi(config =>
                {
                    config.PostProcess = (OpenApiDocument document, Microsoft.AspNetCore.Http.HttpRequest httpRequest) =>
                    {
                        document.Servers.Clear();
                        document.Servers.Add(new OpenApiServer()
                        {
                            Url = string.IsNullOrEmpty(httpRequest.PathBase) ? "/" : httpRequest.PathBase.Value,
                        });
                    };
                });
                app.UseSwaggerUi();
            }

            bool webRootExists = Directory.Exists(Environment.WebRootPath);
            if (webRootExists)
            {
                app.UseStaticFiles(new StaticFileOptions()
                {
                    OnPrepareResponse = (context) =>
                    {
                        if (context.File.Name == "index.html")
                        {
                            context.Context.Response.Headers.CacheControl = "no-cache, no-store, must-revalidate, proxy-revalidate, max-age=0";
                        }
                    }
                });

            }

            app.UseRouting();

            if (Configuration.GetValue<bool>("EnableCors"))
            {
                app.UseCors("Cors");
            }

            //app.UseResponseCaching();

            if (_authenticationConfigured)
            {
                app.UseAuthentication();
                app.UseAuthorization();
            }

            app.MapControllers();

            //if (webRootExists && File.Exists(Path.Combine(env.WebRootPath, "index.html")))
            //{
            //    app.MapFallbackToFile("index.html");
            //}
        }
    }
}

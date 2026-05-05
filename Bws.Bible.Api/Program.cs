using Bws.Bible.Api;
using Bws.Bible.Api.Configuration;
using Bws.Bible.Api.Middlewares;
using Bws.Bible.Core.Configuration;
using Bws.Bible.Core.Repositories;
using Bws.Bible.Infrastructure.Configuration;
using Bws.Bible.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi;
using System;
using System.IO;
using System.Text.Json.Serialization;

static ApiSettings GetApiSettings(IConfiguration configuration)
{
    return new ApiSettings()
    {
        Name = configuration["Api:Name"],
        Version = configuration["Api:Version"],
        BaseUrl = configuration["Api:BaseUrl"],
        Environment = configuration["Api:Environment"],
        GenerateNavigationLinks = bool.Parse(configuration["Api:GenerateNavigationLinks"]),
        GenerateResponseTime = bool.Parse(configuration["Api:GenerateResponseTime"]),
        RegexBibleIdentifier = configuration["Api:RegexBibleIdentifier"],
        SearchWordsMinLength = short.Parse(configuration["Api:SearchWordsMinLength"]),
        SearchWordsMaxLength = short.Parse(configuration["Api:SearchWordsMaxLength"]),
        SearchPaginationEnabled = bool.Parse(configuration["Api:SearchPaginationEnabled"]),
        SearchPaginationVersesPerPage = short.Parse(configuration["Api:SearchPaginationVersesPerPage"]),
        CompareInterlinearVersesEnabled = bool.Parse(configuration["Api:CompareInterlinearVersesEnabled"])
    };
}

static InfrastructureSettings GetInfrastructureSettings(IConfiguration configuration)
{
    return new InfrastructureSettings()
    {
        StoragePath = configuration["Infrastructure:StoragePath"],
        BibleFileExtension = configuration["Infrastructure:BibleFileExtension"],
        BibleFileForHealthCheck = configuration["Infrastructure:BibleFileForHealthCheck"],
        DelimiterSeparatedValues = configuration["Infrastructure:DelimiterSeparatedValues"],
        LoadBiblesAtStartup = bool.Parse(configuration["Infrastructure:LoadBiblesAtStartup"])
    };
}

static CorsSettings GetCorsSettings(IConfiguration configuration)
{
    return new CorsSettings()
    {
        AllowedOrigins = configuration.GetSection("Api:Cors:AllowedOrigins").Get<string[]>() ?? new[] { "*" },
        AllowedMethods = configuration.GetSection("Api:Cors:AllowedMethods").Get<string[]>() ?? new[] { "GET" },
        AllowedHeaders = configuration.GetSection("Api:Cors:AllowedHeaders").Get<string[]>() ?? new[] { "Content-Type" }
    };
}

// Préparation du builder de l'application
var builder = WebApplication.CreateBuilder(args);

var infrastructureSettings = GetInfrastructureSettings(builder.Configuration);
var apiSettings = GetApiSettings(builder.Configuration);

//Cross-Origin (CORS)
var corsSettings = GetCorsSettings(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("ApiCorsPolicy", policy =>
    {
        policy.WithOrigins(corsSettings.AllowedOrigins)
              .WithMethods(corsSettings.AllowedMethods)
              .WithHeaders(corsSettings.AllowedHeaders);
    });
});

builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { "liveness" }) // Liveness : Vérifie si l'api répond                                                                  
    .AddCheck("storage-check", new CriticalFileHealthCheck(Path.Combine(infrastructureSettings.StoragePath, infrastructureSettings.BibleFileForHealthCheck)), tags: new[] { "readiness" }); // Readiness : Vérifie l'accès aux données

// Configuration des services (extrait de ConfigureServices)
builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Repositories
builder.Services.AddSingleton<IBibleRepository, BibleRepository>();

// Configuration
builder.Services.AddSingleton<IInfrastructureSettings>(infrastructureSettings);
builder.Services.AddSingleton<IApiSettings>(apiSettings);

// Swagger
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc(builder.Configuration["Api:Swagger:Version"], new OpenApiInfo
    {
        Title = builder.Configuration["Api:Swagger:Title"],
        Version = builder.Configuration["Api:Swagger:Version"],
        Description = builder.Configuration["Api:Swagger:Description"],
        TermsOfService = new Uri(builder.Configuration["Api:Swagger:TermsOfService"]),
        Contact = new OpenApiContact()
        {
            Name = builder.Configuration["Api:Swagger:Contact:Name"],
            Email = builder.Configuration["Api:Swagger:Contact:Email"],
            Url = new Uri(builder.Configuration["Api:Swagger:Contact:Url"])
        },
        License = new OpenApiLicense()
        {
            Name = builder.Configuration["Api:Swagger:License:Name"],
            Url = new Uri(builder.Configuration["Api:Swagger:License:Url"])
        }
    });
});

// Build de l'application
var app = builder.Build();

// Configuration du pipeline (extrait de Configure)
if (app.Environment.EnvironmentName == Environments.Development)
{
    app.UseDeveloperExceptionPage();
}

if (bool.Parse(builder.Configuration["Api:Security:HttpStrictTransportSecurityEnabled"]))
{
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint($"/swagger/{builder.Configuration["Api:Swagger:Version"]}/swagger.json", builder.Configuration["Api:Swagger:Title"]);
    c.RoutePrefix = "swagger";
    c.DefaultModelsExpandDepth(-1);
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("ApiCorsPolicy");
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<ChronoMiddleware>();
app.UseAuthorization();

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = r => r.Tags.Contains("liveness")
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = r => r.Tags.Contains("readiness")
});

app.MapControllers();

//app.UseMetricServer(); // Expose /metrics
//app.UseHttpMetrics();   // Capture les stats HTTP (latence, codes 200/500)

// Initialisation du repository
app.Services.GetService<IBibleRepository>();

app.Run();
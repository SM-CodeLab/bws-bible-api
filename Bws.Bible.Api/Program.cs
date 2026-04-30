using Bws.Bible.Api.Configuration;
using Bws.Bible.Api.Middlewares;
using Bws.Bible.Core.Configuration;
using Bws.Bible.Core.Repositories;
using Bws.Bible.Infrastructure.Configuration;
using Bws.Bible.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi;
using System;
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
        DelimiterSeparatedValues = configuration["Infrastructure:DelimiterSeparatedValues"],
        LoadBiblesAtStartup = bool.Parse(configuration["Infrastructure:LoadBiblesAtStartup"])
    };
}

// Bible API builder
var builder = WebApplication.CreateBuilder(args);

//Cross-Origin (CORS)
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("ApiCorsPolicy", builder =>
//    {
//        builder.WithOrigins("http://www.samuel-meyer.fr")
//               .WithHeaders(Microsoft.Net.Http.Headers.HeaderNames.ContentType, "x-custom-header")
//               .WithMethods("GET", "OPTIONS"); ;
//    });
//});

// Configuration des services (extrait de ConfigureServices)
builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.IgnoreNullValues = true;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Repositories
builder.Services.AddSingleton<IBibleRepository, BibleRepository>();

// Configuration
builder.Services.AddSingleton<IInfrastructureSettings>(GetInfrastructureSettings(builder.Configuration));
builder.Services.AddSingleton<IApiSettings>(GetApiSettings(builder.Configuration));

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
app.UseCors();
app.UseMiddleware<BwsCorsMiddleware>();
app.UseAuthorization();

app.MapControllers();

// Initialisation du repository
app.Services.GetService<IBibleRepository>();

app.Run();
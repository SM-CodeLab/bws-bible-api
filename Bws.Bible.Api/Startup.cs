using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi;
using Bws.Bible.Core.Configuration;
using Bws.Bible.Core.Repositories;
using Bws.Bible.Infrastructure.Repositories;
using Bws.Bible.Infrastructure.Configuration;
using Bws.Bible.Api.Configuration;
using Bws.Bible.Api.Middlewares;

namespace Bws.Bible.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostEnvironment HostEnvironment { get; private set; }

        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            Configuration = configuration;
            HostEnvironment = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Cross-Origin (CORS)
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("ApiCorsPolicy", builder =>
            //    {
            //        builder.WithOrigins("http://www.samuel-meyer.fr")
            //               .WithHeaders(Microsoft.Net.Http.Headers.HeaderNames.ContentType, "x-custom-header")
            //               .WithMethods("GET", "OPTIONS"); ;
            //    });
            //});

            //MVC
            services.AddMvc()
                .AddJsonOptions(options => { 
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            //Repositories
            services.AddSingleton<IBibleRepository, BibleRepository>();

            //Configuration
            services.AddSingleton<IInfrastructureSettings>(s => GetInfrastructureSettings());
            services.AddSingleton<IApiSettings>(s => GetApiSettings());

            //Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(Configuration["Api:Swagger:Version"], new OpenApiInfo
                {
                    
                    Title = Configuration["Api:Swagger:Title"],
                    Version = Configuration["Api:Swagger:Version"],
                    Description = Configuration["Api:Swagger:Description"],
                    TermsOfService = new Uri(Configuration["Api:Swagger:TermsOfService"]),
                    Contact = new OpenApiContact()
                    {
                        Name = Configuration["Api:Swagger:Contact:Name"],
                        Email = Configuration["Api:Swagger:Contact:Email"],
                        Url = new Uri(Configuration["Api:Swagger:Contact:Url"])
                    },
                    License = new OpenApiLicense()
                    {
                        Name = Configuration["Api:Swagger:License:Name"],
                        Url = new Uri(Configuration["Api:Swagger:License:Url"])
                    }
                });                
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (bool.Parse(Configuration["Api:Security:HttpStrictTransportSecurityEnabled"]))
            {
                app.UseHsts();
                //HTTP Strict Transport Security (HSTS) is a web security policy mechanism which helps to protect websites against protocol downgrade attacks and cookie hijacking.
                //It allows web servers to declare that web browsers (or other complying user agents) should only interact with it using secure HTTPS connections, and never via the insecure HTTP protocol.
                //For example, when you try to access Google with http://www.google.com, the browser will give us 307 status code and will redirect http to https.
            }

            // Serves generated swagger document as JSON endpoint. 
            app.UseSwagger();

            // Serves the Swagger UI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(string.Format("/swagger/{0}/swagger.json", Configuration["Api:Swagger:Version"]), Configuration["Api:Swagger:Title"]); // Swagger JSON endpoint.
                c.RoutePrefix = "swagger";
                c.DefaultModelsExpandDepth(-1); // Disable swagger schemas at bottom
            });

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors();
            app.UseMiddleware<BwsCorsMiddleware>();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Instanciation du BibleRepository pour charger les Bibles en mémoire.
            app.ApplicationServices.GetService<IBibleRepository>();
        }

        private ApiSettings GetApiSettings()
        {
            var config = new ApiSettings()
            {
                Name = Configuration["Api:Name"],
                Version = Configuration["Api:Version"],
                BaseUrl = Configuration["Api:BaseUrl"],
                Environment = Configuration["Api:Environment"],
                GenerateNavigationLinks = bool.Parse(Configuration["Api:GenerateNavigationLinks"]),
                GenerateResponseTime = bool.Parse(Configuration["Api:GenerateResponseTime"]),
                RegexBibleIdentifier = Configuration["Api:RegexBibleIdentifier"],
                SearchWordsMinLength = short.Parse(Configuration["Api:SearchWordsMinLength"]),
                SearchWordsMaxLength = short.Parse(Configuration["Api:SearchWordsMaxLength"]),
                SearchPaginationEnabled = bool.Parse(Configuration["Api:SearchPaginationEnabled"]),
                SearchPaginationVersesPerPage = short.Parse(Configuration["Api:SearchPaginationVersesPerPage"]),
                CompareInterlinearVersesEnabled = bool.Parse(Configuration["Api:CompareInterlinearVersesEnabled"])
            };

            return config;
        }

        private InfrastructureSettings GetInfrastructureSettings()
        {
            var config = new InfrastructureSettings()
            {
                StoragePath = Configuration["Infrastructure:StoragePath"],
                BibleFileExtension = Configuration["Infrastructure:BibleFileExtension"],
                DelimiterSeparatedValues = Configuration["Infrastructure:DelimiterSeparatedValues"],
                LoadBiblesAtStartup = bool.Parse(Configuration["Infrastructure:LoadBiblesAtStartup"])
            };

            return config;
        }
    }
}

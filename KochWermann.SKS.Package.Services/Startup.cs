/*
 * Parcel Logistics Service
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 1.20.1
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using KochWermann.SKS.Package.Services.Filters;
using AutoMapper;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using KochWermann.SKS.Package.BusinessLogic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using KochWermann.SKS.Package.DataAccess.Sql;
using KochWermann.SKS.Package.DataAccess.Interfaces;
using KochWermann.SKS.Package.BusinessLogic.Mapper;
using KochWermann.SKS.Package.Services.Mapper;
using KochWermann.SKS.Package.ServiceAgents;
using KochWermann.SKS.Package.ServiceAgents.Interfaces;

namespace KochWermann.SKS.Package.Services
{
    /// <summary>
    /// Startup
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private readonly IWebHostEnvironment _hostingEnv;
        private IConfiguration _configuration { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="env"></param>
        /// <param name="configuration"></param>
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            _hostingEnv = env;
            _configuration = configuration;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // DAL injection
            services.AddTransient<IParcelRepository, SqlParcelRepository>();
            services.AddTransient<IWarehouseRepository, SqlWarehouseRepository>();
            services.AddTransient<IWebhookRepository, SqlWebhookRepository>();

            // BusinessLogic injection
            services.AddTransient<ITrackingLogic, TrackingLogic>();
            services.AddTransient<IWarehouseLogic, WarehouseLogic>();
            services.AddTransient<IWebhookLogic, WebhookLogic>();

            //ServiceAgents
            services.AddTransient<IGeoEncodingAgent, OpenStreetMapEncodingAgent>();

            //Webhook
            services.AddScoped<IWebhookAgent, RestWebhookAgent>();

            // Automapper
            services.AddAutoMapper(typeof(DalMapperProfile), typeof(BlMapperProfile));

            // Setup Database Connection
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("Database"), x =>
                {
                    x.UseNetTopologySuite();
                    x.MigrationsAssembly("KochWermann.SKS.Package.Services");
                    x.EnableRetryOnFailure(20, new TimeSpan(0, 0, 30), null);
                });
            });


            // Add framework services
            services
                .AddMvc(options =>
                {
                    options.InputFormatters.RemoveType<Microsoft.AspNetCore.Mvc.Formatters.SystemTextJsonInputFormatter>();
                    options.OutputFormatters.RemoveType<Microsoft.AspNetCore.Mvc.Formatters.SystemTextJsonOutputFormatter>();
                })
                .AddNewtonsoftJson(opts =>
                {
                    opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    opts.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
                })
                .AddXmlSerializerFormatters();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("1.20.1", new OpenApiInfo
                {
                    Version = "1.20.1",
                    Title = "Parcel Logistics Service",
                    Description = "Parcel Logistics Service (ASP.NET Core 3.1)",
                    Contact = new OpenApiContact()
                    {
                        Name = "SKS",
                        Url = new Uri("http://www.technikum-wien.at/"),
                        Email = ""
                    },
                });
                c.CustomSchemaIds(type => type.FullName);
                c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{_hostingEnv.ApplicationName}.xml");

                // Include DataAnnotation attributes on Controller Action parameters as Swagger validation rules (e.g required, pattern, ..)
                // Use [ValidateModelState] on Actions to actually validate it in C# as well!
                c.OperationFilter<GeneratePathParamsValidationFilter>();
            });

            services.AddHttpClient("openstreetmap", c =>
            {
                c.BaseAddress = new Uri("https://nominatim.openstreetmap.org/");
                c.DefaultRequestHeaders.Add("User-Agent", "SKS-Koch-Wermann");
            });

            services.AddHttpClient("parcelhop", c =>
            {
                c.DefaultRequestHeaders.Add("User-Agent", "SKS-Koch-Wermann");
            });

            services.AddHttpClient("webhooks", c =>
            {
                c.DefaultRequestHeaders.Add("User-Agent", "SKS-Koch-Wermann");
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="databaseContext"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, DatabaseContext databaseContext)
        {
            databaseContext.Database.Migrate();

            app.UseRouting();

            //TODO: Uncomment this if you need wwwroot folder
            // app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                //TODO: Either use the SwaggerGen generated Swagger contract (generated from C# classes)
                c.SwaggerEndpoint("/swagger/1.20.1/swagger.json", "Parcel Logistics Service");

                //TODO: Or alternatively use the original Swagger contract that's included in the static files
                // c.SwaggerEndpoint("/swagger-original.json", "Parcel Logistics Service Original");
            });

            //TODO: Use Https Redirection
            // app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    //spa.UseAngularCliServer(npmScript: "start");
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //TODO: Enable production exception handling (https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling)
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
        }
    }
}

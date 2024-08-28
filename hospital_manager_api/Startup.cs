using System;
using System.IO;
using System.Reflection;
using voting_api.Configuration;
using voting_data_access.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration;
using Prometheus;
using Microsoft.Extensions.Logging;

namespace voting_api
{
    /// <summary>
    /// Configure les services et le pipeline de requ�tes de l'application.
    /// </summary>
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="Startup"/>.
        /// </summary>
        /// <param name="configuration">La configuration � utiliser.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Obtient la configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configure les services pour l'application.
        /// </summary>
        /// <param name="services">Les services � configurer.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


            // Ajouter DbContext avec SQL Server
            services.AddDbContext<VotingDbContext>(options =>
            {
                // Configure your connection string here
                options.UseNpgsql(connectionString);
            });

            // Ajouter la politique CORS
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins("*")
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            services.AddControllers();
            services.AddMvc();

            // Ajouter Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API Voting Manager",
                    Description = "Cr�ation d'une application de gestion de vote.",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Francesco Bigi",
                        Email = "psr07700@students.ephec.com",
                        Url = new Uri("https://be.linkedin.com/in/bigif"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Utilisation sous LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            // Ajouter d'autres services
            services.AddApplicationServices();
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
        }

        /// <summary>
        /// Configure le pipeline de requ�tes HTTP.
        /// </summary>
        /// <param name="app">Le g�n�rateur d'application.</param>
        /// <param name="env">L'environnement d'h�bergement.</param>
        /// <param name="dbContext">Le contexte de base de donn�es.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, VotingDbContext dbContext)
        {
            dbContext.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Activer le middleware Swagger
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMetricServer();
            app.UseHttpMetrics();

            // Activer l'UI Swagger
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "EPVOTE API V1");
                c.RoutePrefix = string.Empty; // Pour servir l'UI Swagger � la racine de l'application
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

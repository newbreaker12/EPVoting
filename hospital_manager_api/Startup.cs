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
using Npgsql;
using Microsoft.AspNetCore.DataProtection;
using Prometheus;

namespace voting_api
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            //// Do first:
            //// dotnet ef migrations add InitialVotingManagerDefaultDbMigration -c VotingDbContext -o Data/Migrations/DefaultVotingDbContext
            //
            services.AddDbContext<VotingDbContext>(
                options => options.UseSqlServer(connectionString,
                    b => b.MigrationsAssembly("voting_api"))
            );
            //services.AddDbContext<VotingDbContext>(
            //    config =>
            //    {
            //        config.UseInMemoryDatabase("Memory");
            //    }
            //);

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

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("V1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API Voting Manager",
                    Description = "Creation of an Voting Manager application. ",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Francesco Bigi",
                        Email = "psr07700@students.ephec.com",
                        Url = new Uri("https://be.linkedin.com/in/bigif"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddApplicationServices();
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
        }

        /*dotnet ef migrations add InitialVoteManagerDefaultDbMigration -c VoteDbContext -o Data/Migrations/DefaultVoteDbContext
         dotnet ef migrations add InitialVoteManagerDefaultDbMigration -c VoteDbContext -o Data/Migrations/DefaultVoteDbContext*/

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, VotingDbContext dbContext)
        {

            dbContext.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });

            //app.UseSession();
            app.UseHttpsRedirection();
            //app.UseMiddleware<ExceptionMiddleware>();
            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMetricServer();
            app.UseHttpMetrics();

            //Need to use an end point in order to access to swagger page
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/V1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

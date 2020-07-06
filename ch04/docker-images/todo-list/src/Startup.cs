using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus;
using ToDoList.Model;
using ToDoList.Services;

namespace ToDoList
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            var dbReadOnly = Configuration.GetValue<bool>("Database:ReadOnly");
            var connectionString = Configuration.GetConnectionString(dbReadOnly ? "ToDoDb-ReadOnly" : "ToDoDb");

            var dbProvider = Configuration.GetValue<DbProvider>("Database:Provider");
            _ = dbProvider switch
            {
                DbProvider.Sqlite => services.AddDbContext<ToDoContext>(options =>
                     options.UseSqlite(connectionString)),

                DbProvider.Postgres => services.AddDbContext<ToDoContext>(options =>
                     options.UseNpgsql(connectionString, postgresOptions => postgresOptions.EnableRetryOnFailure())),

                _ => throw new NotSupportedException("Supported providers: Sqlite and Posgtres")
            };

            services.AddScoped<ToDoService>();
            services.AddSingleton<DiagnosticsService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePages();             
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/static",
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append(
                         "Cache-Control", $"public, max-age=432000");
                }
            });
            app.UseRouting();

            if (Configuration.GetValue<bool>("Metrics:Enabled"))
            {
                app.UseHttpMetrics();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapRazorPages();
                    endpoints.MapMetrics();
                });
            }
            else
            {
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapRazorPages();
                });
            }
        }
    }
}

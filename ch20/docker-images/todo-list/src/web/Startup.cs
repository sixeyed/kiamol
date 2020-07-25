using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus;
using ToDoList.Messaging;
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
            services.AddToDoContext(Configuration, ServiceLifetime.Scoped);
            services.AddScoped<ToDoService>();
            services.AddSingleton<DiagnosticsService>();
            services.AddSingleton<MessageQueue>();
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

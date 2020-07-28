using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using ToDoList.Model;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddToDoContext(this IServiceCollection services, IConfiguration config, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            var dbReadOnly = config.GetValue<bool>("Database:ReadOnly");
            var connectionString = config.GetConnectionString(dbReadOnly ? "ToDoDb-ReadOnly" : "ToDoDb");

            var dbProvider = config.GetValue<DbProvider>("Database:Provider");
            _ = dbProvider switch
            {
                DbProvider.Sqlite => services.AddDbContext<ToDoContext>(options =>
                     options.UseSqlite(connectionString),
                     lifetime),

                DbProvider.Postgres => services.AddDbContext<ToDoContext>(options =>
                     options.UseNpgsql(connectionString, postgresOptions => postgresOptions.EnableRetryOnFailure()),
                     lifetime),

                DbProvider.MySql => services.AddDbContext<ToDoContext>(options =>
                     options.UseMySQL(connectionString),
                     lifetime),

                _ => throw new NotSupportedException("Supported providers: Sqlite, Posgtres and MySQL")
            };

            return services;
        }
    }
}

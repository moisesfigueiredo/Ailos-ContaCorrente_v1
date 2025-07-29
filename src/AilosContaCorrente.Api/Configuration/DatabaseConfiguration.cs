using AilosContaCorrente.Domain.Entities;
using AilosContaCorrente.PostgresDB.Core;
using Microsoft.EntityFrameworkCore;

namespace AilosContaCorrente.Api.Configuration
{
    public static class DatabaseConfiguration
    {
        public static void AddDatabaseConfiguration(this WebApplicationBuilder builder)
        {
            //Configure Postgres Database
            builder.Services.Configure<DatabaseSettings>(
                builder.Configuration.GetSection("DatabaseSettings")
            );

            var migrationsAssembly = typeof(ApplicationDbContext).Assembly.GetName().Name;
            var migrationTable = "__ProdutoMigrationsHistory";
            var databaseSettings = builder.Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(databaseSettings.ConnectionString, b =>
                {
                    b.MigrationsAssembly(migrationsAssembly);
                    b.MigrationsHistoryTable(migrationTable);
                });

                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            });
        }

        public static void AddMigrationsConfiguration(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.Migrate();
            }

        }
    }
}

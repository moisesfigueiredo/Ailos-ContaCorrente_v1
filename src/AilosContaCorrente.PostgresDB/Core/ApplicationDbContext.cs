using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AilosContaCorrente.PostgresDB.Core
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MapEntities(modelBuilder);
            DisableDeleteCascade(modelBuilder);

            SetDefaultDatabaseTypes(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        protected void SetDefaultDatabaseTypes(ModelBuilder modelBuilder)
        {
            //Int64 = bigint
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties().Where(p => p.ClrType == typeof(long) || p.ClrType == typeof(long?))))
                property.SetColumnType("bigint");
        }

        protected void DisableDeleteCascade(ModelBuilder modelBuilder)
        {
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        protected void MapEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

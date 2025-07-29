using AilosContaCorrente.Domain.Abstractions;
using AilosContaCorrente.PostgresDB.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AilosContaCorrente.IoC
{
    public static class RootBootstrapper
    {
        public static void BootstrapperRegisterServices(this IServiceCollection services)
        {
            var assemblyTypes = typeof(RootBootstrapper).Assembly.GetNoAbstractTypes();

            services.AddImplementations(ServiceLifetime.Scoped, typeof(IBaseRepository), assemblyTypes);

            //Repositories postgresDB
            services.AddScoped<IContaCorrenteRepository, ContaCorrenteRepository>();
            services.AddScoped<IMovimentoRepository, MovimentoRepository>();

            var handlers = AppDomain.CurrentDomain.Load("AilosContaCorrente.Application");
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(handlers));
        }
    }
}
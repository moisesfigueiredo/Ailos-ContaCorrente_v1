using AilosContaCorrente.Api.Services;
using AilosContaCorrente.IoC;

namespace AilosContaCorrente.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            RootBootstrapper.BootstrapperRegisterServices(services);

            services.AddScoped<ContaCorrenteAuthService>();
        }
    }
}

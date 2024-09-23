using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Plc.Command;
public static class DependencyInjection
{
    public static IServiceCollection AddPlcCommand(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PlcOptions>(configuration.GetSection("PlcConfig"));
        services.AddSingleton<IPlcService, PlcService>();

        return services;
    }
}

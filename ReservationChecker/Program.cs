using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ReservationChecker;

class Program
{
    static async Task Main(string[] args)
    {
        using IHost host = CreateHostBuilder(args).Build();

        await host.RunAsync();
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.AddHostedService<AplicationHostedService>();
        });
}

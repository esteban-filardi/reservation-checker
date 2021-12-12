using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ReservationChecker;

class Program
{
    static async Task Main(string[] args)
    {
        IHostBuilder hostBuilder = CreateHostBuilder(args);                
        
        using IHost host = hostBuilder.Build();        

        await host.RunAsync();
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.AddHostedService<AplicationHostedService>();
        });
}

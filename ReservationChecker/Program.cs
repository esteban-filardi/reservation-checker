using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReservationChecker.Application;

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
            services.AddSingleton<IServiceScrapper, ServiceScrapper.ServiceScrapper>();
            services.AddSingleton<IInterestingServicesFinder, InterestingServicesFinder>();
            services.AddSingleton<Application.ReservationChecker>();

            services.AddHostedService<AplicationHostedService>();

        });
}

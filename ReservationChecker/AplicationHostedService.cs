using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationChecker;

public class AplicationHostedService : IHostedService
{
    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _appLifetime;

    public AplicationHostedService(
        ILogger<AplicationHostedService> logger,
        IHostApplicationLifetime appLifetime)
    {
        _logger = logger;
        _appLifetime = appLifetime;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _appLifetime.ApplicationStarted.Register(async () =>
        {
            try
            {
                _logger.LogInformation($"Starting");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
            }
            finally
            {
                // Stop the application once the work is done
                _appLifetime.StopApplication();
            }
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

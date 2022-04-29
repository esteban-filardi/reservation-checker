using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationChecker.Infrastructure;

public class AplicationHostedService : IHostedService
{
    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _appLifetime;
    private readonly Application.ReservationChecker _reservationChecker;
    private Timer _timer = null!;

    public AplicationHostedService(
        ILogger<AplicationHostedService> logger,
        IHostApplicationLifetime appLifetime,
        Application.ReservationChecker reservationChecker)
    {
        _logger = logger;
        _appLifetime = appLifetime;
        _reservationChecker = reservationChecker;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _appLifetime.ApplicationStarted.Register(() =>
        {
            try
            {
                _timer = new Timer(async (state) =>
                {
                    try
                    {
                        await _reservationChecker.Execute();
                    } catch (Exception exception)
                    {
                        _logger.LogError(exception.ToString());
                    }
                }, null, TimeSpan.Zero, TimeSpan.FromMinutes(15));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
            }
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Application Hosted Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }
}

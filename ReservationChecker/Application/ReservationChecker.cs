using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ReservationChecker.ServiceScrapper;
using ReservationChecker.ServiceScrapper.Models;

namespace ReservationChecker.Application;

public class ReservationChecker
{
    private readonly ILogger _logger;
    private readonly IConfiguration _config;
    private readonly IInterestingServicesFinder _interestingServicesFinder;
    private readonly IEmailNotifier _emailNotifier;

    public ReservationChecker(
        ILogger<ReservationChecker> logger,
        IConfiguration config,
        IInterestingServicesFinder interestingServicesFinder,
        IEmailNotifier emailNotifier)
    {
        _logger = logger;
        _config = config;
        _interestingServicesFinder = interestingServicesFinder;
        _emailNotifier = emailNotifier;
    }

    const string loggerTemplate = $"New interesting services available!{{ServiceDetails}}";

    public async Task<int> Execute()
    {
        IEnumerable<Service> interestingServices = await _interestingServicesFinder.FindInterestingServices();

        if (interestingServices.Any())
        {
            LogInterestServicesFound(interestingServices);
            _emailNotifier.NotifyInterestingServices(interestingServices);
        }
        else
        {
            _logger.LogInformation("No interesting services available");
        }

        return 0;
    }

    private void LogInterestServicesFound(IEnumerable<Service> interestingServices)
    {
        var serviceDetailsBuilder = new StringBuilder();
        serviceDetailsBuilder.AppendLine();
        foreach (var service in interestingServices)
        {
            serviceDetailsBuilder.AppendLine();
            serviceDetailsBuilder.AppendLine(service.ToString());            
        }

        _logger.LogInformation(loggerTemplate, serviceDetailsBuilder.ToString());
    }
}

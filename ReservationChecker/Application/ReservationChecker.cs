using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ReservationChecker.Application;
using ReservationChecker.ServiceScrapper;
using ReservationChecker.ServiceScrapper.Models;

namespace ReservationChecker.Checker
{
    public class ReservationChecker
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        private readonly InterestingServicesFinder _interestingServicesFinder;

        public ReservationChecker(
            ILogger<ReservationChecker> logger,
            IConfiguration config,
            InterestingServicesFinder interestingServicesFinder)
        {
            _logger = logger;
            _config = config;
            _interestingServicesFinder = interestingServicesFinder;
        }

        public async Task<int> Execute()
        {
            IEnumerable<RetrieveServicesResponse> interestingServices = await _interestingServicesFinder.FindInterestingServices();

            if (!interestingServices.Any())
            {
                Console.WriteLine("No interesting service available");
            } else
            {
                Console.WriteLine("Interesting services available");
            }

            return 0;
        }
    }
}

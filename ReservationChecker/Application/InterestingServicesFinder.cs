using ReservationChecker.ServiceScrapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationChecker.Application
{
    public class InterestingServicesFinder : IInterestingServicesFinder
    {

        private readonly HashSet<int> UnrelevantServicesIds = new() { 554, 699, 901 };
        private readonly IServiceScrapper _serviceScrapper;

        public InterestingServicesFinder(IServiceScrapper serviceScrapper)
        {
            _serviceScrapper = serviceScrapper;
        }

        public async Task<IEnumerable<Service>> FindInterestingServices()
        {
            IEnumerable<Service> availableServices = await _serviceScrapper.RetrieveServices();

            return availableServices.Where(s => !s.ServiceId.HasValue || !UnrelevantServicesIds.Contains(s.ServiceId.Value));
        }
    }
}

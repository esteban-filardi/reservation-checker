using ReservationChecker.ServiceScrapper.Models;

namespace ReservationChecker.Application;

public interface IInterestingServicesFinder
{
    Task<IEnumerable<Service>> FindInterestingServices();
}

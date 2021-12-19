using ReservationChecker.ServiceScrapper.Models;

namespace ReservationChecker.Application;

public interface IServiceScrapper
{
    Task<IEnumerable<Service>> RetrieveServices();
}

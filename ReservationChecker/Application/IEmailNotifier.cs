using ReservationChecker.ServiceScrapper.Models;

namespace ReservationChecker.Application;

public interface IEmailNotifier
{
    public void NotifyInterestingServices(IEnumerable<Service> services);
}
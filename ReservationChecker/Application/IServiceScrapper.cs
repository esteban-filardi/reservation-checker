using ReservationChecker.ServiceScrapper.Models;

namespace ReservationChecker.ServiceScrapper
{
    public interface IServiceScrapper
    {
        Task<IEnumerable<RetrieveServicesResponse>> RetrieveServices();
    }
}
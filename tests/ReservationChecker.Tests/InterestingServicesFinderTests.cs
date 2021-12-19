using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Xunit;
using System.Threading.Tasks;
using ReservationChecker.ServiceScrapper.Models;
using ReservationChecker.Application;
using System.Linq;

namespace ReservationChecker.Tests;

public class InterestingServicesFinderTests
{
    [Fact]
    public async void Should_return_empty_array_when_there_are_no_interesting_services()
    {
        var serviceScrapperMock = new Moq.Mock<IServiceScrapper>();

        IEnumerable<Service> mockedResult = new List<Service>()
        {
            new Service() {ServiceId = 554 },
            new Service() {ServiceId = 699 },
            new Service() {ServiceId = 901 },
        };

        serviceScrapperMock.Setup(o => o.RetrieveServices()).Returns(Task.FromResult(mockedResult));

        var reservationChecker = new InterestingServicesFinder(serviceScrapperMock.Object);

        IEnumerable<Service> interestingServices = await reservationChecker.FindInterestingServices();

        Assert.Empty(interestingServices);
    }

    [Fact]
    public async void Should_return_array_with_interesting_services_when_this_are_available()
    {
        var serviceScrapperMock = new Moq.Mock<IServiceScrapper>();

        Service interestingService = new Service() { ServiceId = 500 };
        IEnumerable<Service> mockedResult = new List<Service>()
        {
            new Service() { ServiceId = 554 },
            new Service() { ServiceId = 699 },
            interestingService,
        };

        serviceScrapperMock.Setup(o => o.RetrieveServices()).Returns(Task.FromResult(mockedResult));

        var reservationChecker = new InterestingServicesFinder(serviceScrapperMock.Object);

        IEnumerable<Service> interestingServices = await reservationChecker.FindInterestingServices();

        Assert.Collection(interestingServices, new System.Action<Service>[] {
            (service) => Assert.Equal(service, interestingService)
        });
    }

}

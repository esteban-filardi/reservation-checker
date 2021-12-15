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
        var serviceScrapperMock = new Moq.Mock<ServiceScrapper.IServiceScrapper>();

        IEnumerable<RetrieveServicesResponse> mockedResult = new List<RetrieveServicesResponse>()
        {
            new RetrieveServicesResponse() {ServiceId = 554 },
            new RetrieveServicesResponse() {ServiceId = 699 },
            new RetrieveServicesResponse() {ServiceId = 901 },
        };

        serviceScrapperMock.Setup(o => o.RetrieveServices()).Returns(Task.FromResult(mockedResult));

        var reservationChecker = new InterestingServicesFinder(serviceScrapperMock.Object);

        IEnumerable<RetrieveServicesResponse> interestingServices = await reservationChecker.FindInterestingServices();

        Assert.Empty(interestingServices);
    }

    [Fact]
    public async void Should_return_array_with_interesting_services_when_this_are_available()
    {
        var serviceScrapperMock = new Moq.Mock<ServiceScrapper.IServiceScrapper>();

        RetrieveServicesResponse interestingService = new RetrieveServicesResponse() { ServiceId = 500 };
        IEnumerable<RetrieveServicesResponse> mockedResult = new List<RetrieveServicesResponse>()
        {
            new RetrieveServicesResponse() { ServiceId = 554 },
            new RetrieveServicesResponse() { ServiceId = 699 },
            interestingService,
        };

        serviceScrapperMock.Setup(o => o.RetrieveServices()).Returns(Task.FromResult(mockedResult));

        var reservationChecker = new InterestingServicesFinder(serviceScrapperMock.Object);

        IEnumerable<RetrieveServicesResponse> interestingServices = await reservationChecker.FindInterestingServices();

        Assert.Collection(interestingServices, new System.Action<RetrieveServicesResponse>[] {
            (service) => Assert.Equal(service, interestingService)
        });
    }

}

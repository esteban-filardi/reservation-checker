using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using ReservationChecker.Application;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using ReservationChecker.ServiceScrapper.Models;

namespace ReservationChecker.Tests;

public class ReservationCheckerTests
{
    private readonly Mock<ILogger<Application.ReservationChecker>> _loggerMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<IInterestingServicesFinder> _interestingServiceFinder;

    public ReservationCheckerTests()
    {
        _loggerMock = new Mock<ILogger<Application.ReservationChecker>>();
        _configurationMock = new Mock<IConfiguration>();
        _interestingServiceFinder = new Mock<IInterestingServicesFinder>();
    }

    private Application.ReservationChecker CreateReservationChecker()
    {
        IEnumerable<Service> interestingServices = new List<Service>()
        {
            new Service {ServiceId = 1}
        };

        _interestingServiceFinder.Setup(x => x.FindInterestingServices()).Returns(Task.FromResult(interestingServices));

        var reservationChecker = new Application.ReservationChecker(_loggerMock.Object,
            _configurationMock.Object,
            _interestingServiceFinder.Object);
        return reservationChecker;
    }

    [Fact]
    async void Should_print_a_log_when_there_is_new_service_available()
    {
        Application.ReservationChecker reservationChecker = CreateReservationChecker();
        await reservationChecker.Execute();

        _loggerMock.Verify(
            l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("New interesting services available!")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}

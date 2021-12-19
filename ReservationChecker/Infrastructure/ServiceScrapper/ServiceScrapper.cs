using Microsoft.Extensions.Configuration;
using ReservationChecker.Application;
using ReservationChecker.ServiceScrapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ReservationChecker.ServiceScrapper;

public class ServiceScrapper : IServiceScrapper
{
    private readonly HttpClient httpClient;
    private readonly IConfiguration _config;
    private readonly string _serviceUrl;

    public ServiceScrapper(IConfiguration config)
    {
        HttpClientHandler httpClientHandler = new() { UseCookies = true };

        httpClient = new HttpClient(httpClientHandler);

        _config = config;
        _serviceUrl = _config["ServiceApiUrl"];
    }

    Task<HttpResponseMessage> PerformLogin()
    {
        var request = new HttpRequestMessage(new HttpMethod("POST"), $"{_serviceUrl}/Home/Login");

        var contentList = new List<string>
            {
                $"Email={Uri.EscapeDataString(_config["Email"])}",
                $"Password={Uri.EscapeDataString(_config["Password"])}"
            };
        request.Content = new StringContent(string.Join("&", contentList));
        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

        return httpClient.SendAsync(request);
    }

    public async Task<IEnumerable<Service>> RetrieveServices()
    {
        await PerformLogin();

        var servicesEndpointUrl = $"{_serviceUrl}/Services/RetrieveServices";

        var request = new HttpRequestMessage(new HttpMethod("GET"), servicesEndpointUrl);

        return await httpClient.GetFromJsonAsync<IEnumerable<Service>>(request.RequestUri) ??
            new List<Service>();
    }
}

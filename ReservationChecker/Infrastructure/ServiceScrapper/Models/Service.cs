using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReservationChecker.ServiceScrapper.Models;

public class Service
{
    public int? ServiceId { get; set; }
    public string? ProviderServiceDescription { get; set; }
    public override string? ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Service Id: {ServiceId}");
        sb.AppendLine($"Description: {ProviderServiceDescription}");

        return sb.ToString();
    }
}

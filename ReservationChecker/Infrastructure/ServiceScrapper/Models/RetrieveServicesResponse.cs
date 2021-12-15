using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReservationChecker.ServiceScrapper.Models;

public class RetrieveServicesResponse
{
    [JsonPropertyName("IdServizioErogato")]
    public int? ServiceId { get; set; }
}

using MailKit.Net.Smtp;
using MimeKit;
using ReservationChecker.Application;
using ReservationChecker.ServiceScrapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationChecker.Infrastructure
{
    public class EmailNotifier : IEmailNotifier
    {
        private readonly IConfiguration _config;

        public EmailNotifier(IConfiguration config)
        {
            _config = config;
        }

        public void NotifyInterestingServices(IEnumerable<Service> services)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Reservation Checker Service", _config["EmailService:SenderMailbox:Address"]));
            message.To.Add(new MailboxAddress(_config["EmailService:ReceiverMailbox:Name"], _config["EmailService:ReceiverMailbox:Address"]));
            message.Subject = "Interesting Services Available";

            message.Body = new TextPart("plain")
            {
                Text = GetEmailText(services)
            };

            using var client = new SmtpClient();
            client.Connect(_config["EmailService:ServerAddress"], 587);

            client.Authenticate(_config["EmailService:ServerUsername"], _config["EmailService:ServerPassword"]);

            client.Send(message);
            client.Disconnect(true);
        }

        private static string GetEmailText(IEnumerable<Service> services)
        {
            var sb = new StringBuilder();
            sb.AppendLine("New interesting services available:");
            foreach (Service service in services) sb.AppendLine(service.ToString());

            return sb.ToString();
        }
    }
}

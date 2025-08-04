using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using System.Text;
using TripTracker.Services.EmailApi.Models.Dto;
using TripTracker.Services.TripApi.Data;

namespace TripTracker.Services.EmailApi.Services
{
    public class EmailService : IEmailService
    {
        //private DbContextOptions<EmailDbContext> _emailDbOptions;

        //public EmailService(DbContextOptions<EmailDbContext> emailDbOptions)
        //{
        //    //_emailDbOptions = emailDbOptions;
        //}

        public void SendTripCreatedEmail(TripDto tripDto)
        {
            StringBuilder emailContent = new StringBuilder();

            emailContent.AppendLine($"<br/>Dear Friends");
            emailContent.AppendLine("<br/>");
            emailContent.AppendLine($"<br/>I have created a trip as we discussed earlier. " +
                $"We start from {tripDto.From} and travel towards {tripDto.To}.");
            emailContent.AppendLine("<br/>");
            emailContent.AppendLine("<br/>Participants:");
            emailContent.AppendLine("<br/><ul>");
            foreach (var participant in tripDto.Participants)
            {
                emailContent.AppendLine($"<li>{participant}</li>");
            }
            emailContent.AppendLine("</ul>");
            emailContent.AppendLine("<br/>See you all at the meeting point.");
            emailContent.AppendLine("<br/>Thanks and Regards,");
            emailContent.AppendLine("<br/>Raja :)");

            SendEmail(emailContent.ToString());
        }

        private void SendEmail(string emailMessage)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("ntraja11@gmail.com");

                mail.To.Add("ntraja11@outlook.com");
                mail.Subject = "Trip Organized";
                mail.Body = emailMessage;

                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("ntraja11@gmail.com", Environment.GetEnvironmentVariable("SMTP-KEY"));
                smtp.EnableSsl = true;

                smtp.Send(mail);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
        
    }
}

using DotnetApiTemplate.Core.Models.Notification;
using DotnetApiTemplate.Infrastructure.Services.Notification;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace DotnetApiTemplate.WebApi.Endpoints.Payment.Notification
{
    public class PaymentSuccessService : NotificationService
  {
    protected async override Task<SendEmailRequest> SendEmailNotification()
    {
      var item = new SendEmailRequest
      {
        IsHTML = false,
        Content = "hi hi",
        Subject = "-",
        Tos = new List<EmailAddress>()
        {
          new EmailAddress("wardah.rose123@gmail.com", "Example User")
        }
      };

      return item;
    }

    protected async override Task<SendEmailRequest> SendPushNotification()
    {
      var item = new SendEmailRequest
      {
        IsHTML = false,
        Content = "hi hi",
        Subject = "-",
        Tos = new List<EmailAddress>()
        {
          new EmailAddress("wardah.rose123@gmail.com", "Example User")
        }
      };

      return item;
    }
  }
}

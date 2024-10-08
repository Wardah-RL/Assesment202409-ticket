﻿using DotnetApiTemplate.Core.Models.Notification;
using DotnetApiTemplate.Core.Models.Queue;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Infrastructure.Services.Notification;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.Shared.Abstractions.Models;
using HandlebarsDotNet;
using Microsoft.EntityFrameworkCore;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading;

namespace DotnetApiTemplate.WebApi.Endpoints.Payment.Notification
{
  public class PaymentSuccessService : NotificationService
  {
    private IDictionary<string, object> _notificationData;
    private List<Guid> Recepient =  new List<Guid>();

    public PaymentSuccessService(IServiceProvider serviceProvider, QueueConfiguration queueConfiguration, SendGridConfiguration sendGridConfiguration) : base(serviceProvider, queueConfiguration, sendGridConfiguration)
    {
    }

    protected async override Task Prepare(IDbContext dbContext, CancellationToken cancellationToken, NotificationRequest request)
    {

      var getBookingTicket = await dbContext.Set<TrBookingTicketBroker>()
                              .Include(e=>e.User)
                              .Include(e=>e.EventBroker)
                              .Where(e => e.Id == request.Id)
                              .FirstOrDefaultAsync(cancellationToken);

      if (getBookingTicket == null)
        throw new Exception("booking ticket is not found");

      _notificationData = new Dictionary<string, object>
                            {
                                //{ "recepientName", getBookingTicket.User.FullName},
                                //{ "recepientEmail", getBookingTicket.User.Email },
                                { "event", getBookingTicket.EventBroker.Name },
                                { "date", getBookingTicket.DateEvent.ToString("dd MMM yyyy")},
                                { "countTicket", getBookingTicket.CountTicket},
                                { "price", getBookingTicket.EventBroker.Price },
                                { "total", (getBookingTicket.CountTicket * getBookingTicket.EventBroker.Price) }
                            };

      Recepient = new List<Guid>()
      {
        getBookingTicket.User.Id
      };
    }

    protected async override Task<SendEmailRequest> SendEmailNotification(IDbContext dbContext, CancellationToken cancellationToken,SendEmailRequest request)
    {
      var getRecepient = await dbContext.Set<User>()
                             .Where(e => Recepient.Contains(e.Id))
                             .Select(e => new EmailAddress
                             {
                               Email = e.Email,
                               Name = e.FullName
                             })
                             .FirstOrDefaultAsync(cancellationToken);

      _notificationData["recepientName"] = getRecepient.Name;

      var compileHtml = Handlebars.Compile(request.HtmlContent);
      var contentHtml = compileHtml(_notificationData);

      var compileText = Handlebars.Compile(request.TextContent);
      var contentText = compileText(_notificationData);

      var compileSubject = Handlebars.Compile(request.Subject);
      var contentSubject = compileSubject(_notificationData);

      var item = new SendEmailRequest
      {
        IsHTML = request.IsHTML,
        HtmlContent = contentHtml,
        TextContent = contentText,
        Subject = contentSubject,
        Tos = new List<EmailAddress>()
        {
          getRecepient
        }
      };

      return item;
    }

    protected async override Task<SendEmailRequest> SendPushNotification()
    {
      //var item = new SendEmailRequest
      //{
      //  IsHTML = false,
      //  Content = "hi hi",
      //  Subject = "-",
      //  Tos = new List<EmailAddress>()
      //  {
      //    new EmailAddress("wardah.rose123@gmail.com", "Example User")
      //  }
      //};

      return default;
    }
  }
}

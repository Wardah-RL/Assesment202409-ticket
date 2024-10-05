using Azure.Core;
using Azure.Storage.Queues;
using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Core.Models.Queue;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Domain.Enums;
using DotnetApiTemplate.Shared.Abstractions.Contexts;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.Shared.Abstractions.Helpers;
using DotnetApiTemplate.Shared.Abstractions.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DotnetApiTemplate.Infrastructure.Services.Queue
{
    public class PaymetQueueService : ReciverBaseQueueService
  {
    private readonly ISendQueue _emailQueue;
    private readonly IServiceProvider _serviceProvider;

    public PaymetQueueService(QueueConfiguration queueConfiguration, IServiceProvider serviceProvider, ISendQueue emailQueue) : base(queueConfiguration, serviceProvider)
    {
      _emailQueue = emailQueue;
      _serviceProvider = serviceProvider;
    }

    public async override Task<bool> QueueContent(IDbContext dbContext, CancellationToken cancellationToken, string scenario, SendQueueRequest message)
    {
      try
      {
        var getPaymentMessage = JsonConvert.DeserializeObject<PaymentMessageQueueRequest>(message.Message);

        var getBookingTicket = await dbContext.Set<TrBookingTicket>()
                         .Where(e => e.Id == getPaymentMessage.OrderCode)
                         .FirstOrDefaultAsync(cancellationToken);

        if (getBookingTicket != null)
        {
          dbContext.AttachEntity(getBookingTicket);
          getBookingTicket.Status = BookingOrderStatus.Done;

          var newBooking = new TrPayment
          {
            Id = new UuidV7().Value,
            TotalPayment = getPaymentMessage.TotalPayment,
            NamaPengirim = getPaymentMessage.NamaPengirim,
            Bank = getPaymentMessage.Bank,
            IdBookingTicket = getBookingTicket.Id,
          };
          await dbContext.InsertAsync(newBooking, cancellationToken);
          await dbContext.SaveChangesAsync(cancellationToken);

          #region
          //var apiKey = "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";
          //var client = new SendGridClient(apiKey);
          //var from = new EmailAddress("wardah@radyalabs.id", "Example User");
          //var subject = "Sending with SendGrid is Fun";
          //var to = new EmailAddress("wardah.rose123@gmail.com", "Example User");
          //var plainTextContent = "and easy to do anywhere, even with C#";
          //var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
          //var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
          //var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
          //var coba = "";
          #endregion


          #region MessageBroker
          NotificationRequest getNotification = new NotificationRequest
          {
            Scenario = "T001",
            Id = getPaymentMessage.IdBookingTicket
          };

          SendQueueRequest _paramQueue = new SendQueueRequest
          {
            Message = JsonSerializer.Serialize(getNotification),
            Scenario = "Notification",
            QueueName = "notification"
          };

          _emailQueue.Execute(_paramQueue);
          #endregion
        }

        return true;
      }
      catch (Exception ex)
      {
        var getPaymentMessage = JsonConvert.DeserializeObject<PaymentMessageQueueRequest>(message.Message);

        var getBookingTicket = await dbContext.Set<TrBookingTicket>()
                         .Where(e => e.Id == getPaymentMessage.IdBookingTicket)
                         .FirstOrDefaultAsync(cancellationToken);

        if (getBookingTicket != null)
        {
          dbContext.AttachEntity(getBookingTicket);
          getBookingTicket.Status = BookingOrderStatus.failed;
          await dbContext.SaveChangesAsync(cancellationToken);
        }

        //#region MessageBroker
        //BookingTicketFeedbackQueueRequest getBookingFeedback = new BookingTicketFeedbackQueueRequest
        //{
        //  IdBookingTicketBroker = getBookingMessage.IdBookingTicketBroker,
        //  Status = BookingOrderStatus.failed,
        //};

        //SendQueueRequest _paramQueue = new SendQueueRequest
        //{
        //  Message = JsonSerializer.Serialize(getBookingFeedback),
        //  Scenario = "BookingFeedback",
        //  QueueName = "bookingfeedback"
        //};

        //_emailQueue.Execute(_paramQueue);
        //#endregion

        return false;
      }
    }
  }
}

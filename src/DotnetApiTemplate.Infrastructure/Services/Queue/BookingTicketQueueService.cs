using Azure.Core;
using Azure.Storage.Queues;
using DotnetApiTemplate.Core.Abstractions.Queue;
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
  public class BookingTicketQueueService : ReciverBaseQueueService
  {
    private readonly ISendQueue _emailQueue;

    public BookingTicketQueueService(QueueConfiguration queueConfiguration, IServiceProvider serviceProvider, ISendQueue emailQueue) : base(queueConfiguration, serviceProvider)
    {
      _emailQueue = emailQueue;
    }

    public async override Task<bool> QueueContent(IDbContext dbContext, CancellationToken cancellationToken, string scenario, SendQueueRequest message)
    {
      try
      {
        var getBookingMessage = JsonConvert.DeserializeObject<BookingTicketQueueRequest>(message.Message);

        var getEvent = await dbContext.Set<MsEvent>()
                         .Where(e => e.Id == getBookingMessage.IdEvent)
                         .FirstOrDefaultAsync(cancellationToken);
        if (getEvent != null)
        {
          if (getEvent.CountTicket == 0)
            throw new Exception("Ticket is sold out");

          if (getEvent.CountTicket < getBookingMessage.CountTicket)
            throw new Exception($"Ticket already is {getEvent.CountTicket}");

          dbContext.AttachEntity(getEvent);
          getEvent.CountTicket = getEvent.CountTicket - getBookingMessage.CountTicket;

          var newBooking = new TrBookingTicket
          {
            Id = new UuidV7().Value,
            FullName = getBookingMessage.Name,
            Email = getBookingMessage.Email,
            CountTicket = getBookingMessage.CountTicket,
            phone = getBookingMessage.Phone,
            Status = BookingOrderStatus.Payment,
            IdBookingTicketBroker = getBookingMessage.IdBookingTicketBroker,
            IdEvent = getBookingMessage.IdEvent,
            DateEvent = getBookingMessage.DateEvent,
          };
          await dbContext.InsertAsync(newBooking, cancellationToken);
          await dbContext.SaveChangesAsync(cancellationToken);

          #region MessageBroker
          BookingTicketFeedbackQueueRequest getBookingFeedback = new BookingTicketFeedbackQueueRequest
          {
            IdBookingTicketBroker = getBookingMessage.IdBookingTicketBroker,
            Status = newBooking.Status,
            OrderCode = newBooking.Id,
          };

          SendQueueRequest _paramQueue = new SendQueueRequest
          {
            Message = JsonSerializer.Serialize(getBookingFeedback),
            Scenario = "BookingFeedback",
            QueueName = "bookingfeedback"
          };

          _emailQueue.Execute(_paramQueue);
          #endregion
        }

        return true;
      }
      catch (Exception ex)
      {
        #region MessageBroker
        var getBookingMessage = JsonConvert.DeserializeObject<BookingTicketQueueRequest>(message.Message);

        BookingTicketFeedbackQueueRequest getBookingFeedback = new BookingTicketFeedbackQueueRequest
        {
          IdBookingTicketBroker = getBookingMessage.IdBookingTicketBroker,
          Status = BookingOrderStatus.Filed,
        };

        SendQueueRequest _paramQueue = new SendQueueRequest
        {
          Message = JsonSerializer.Serialize(getBookingFeedback),
          Scenario = "BookingFeedback",
          QueueName = "bookingfeedback"
        };

        _emailQueue.Execute(_paramQueue);
        #endregion

        return false;
      }
    }
  }
}

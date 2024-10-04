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
  public class BookingTicketQueueService : IGetQueue
  {
    private readonly QueueConfiguration _queueConfiguration;
    private readonly CancellationToken cancellationToken;
    private readonly IServiceProvider _serviceProvider;
    private readonly ISendQueue _emailQueue;

    public BookingTicketQueueService(QueueConfiguration queueConfiguration, IServiceProvider serviceProvider, ISendQueue emailQueue)
    {
      _queueConfiguration = queueConfiguration;
      _serviceProvider = serviceProvider;
      _emailQueue = emailQueue;
    }

    public async void Execute(string queueName)
    {
      string connectionString = _queueConfiguration.Connection;

      QueueClient queue = new QueueClient(connectionString, queueName);

      if (queue.Exists())
      {
        try
        {
          var listReceiveMessages = queue.ReceiveMessages(maxMessages: 32).Value.ToList();

          foreach (var message in listReceiveMessages)
          {
            var jsonString = message.Body.ToString();
            var getMessage = JsonConvert.DeserializeObject<SendQueueRequest>(jsonString);
            if (getMessage == null)
              continue;

            if (getMessage.Message == null)
              continue;

            var getBookingMessage = JsonConvert.DeserializeObject<BookingTicketQueueRequest>(getMessage.Message);

            try
            {
              using (var scope = _serviceProvider.CreateScope())
              {
                var dbContext = scope.ServiceProvider.GetRequiredService<IDbContext>();

                var getEvent = await dbContext.Set<MsEvent>()
                               .Where(e => e.Id == getBookingMessage.IdEvent)
                               .FirstOrDefaultAsync(cancellationToken);

                if (getEvent != null)
                {
                  dbContext.AttachEntity(getEvent);
                  getEvent.CountTicket = getEvent.CountTicket - getBookingMessage.CountTicket;

                  var newBooking = new TrBookingTicket
                  {
                    Id = new UuidV7().Value,
                    FullName = getBookingMessage.Name,
                    Email = getBookingMessage.Email,
                    CountTicket = getBookingMessage.CountTicket,
                    phone = getBookingMessage.Phone,
                    Status = BookingOrderStatus.Pay,
                    IdBookingTicketBroker = getBookingMessage.IdBookingTicketBroker,
                  };
                  await dbContext.InsertAsync(newBooking, cancellationToken);
                  await dbContext.SaveChangesAsync(cancellationToken);

                  #region MessageBroker
                  BookingTicketFeedbackQueueRequest getBookingFeedback = new BookingTicketFeedbackQueueRequest
                  {
                    IdBookingTicketBroker = getBookingMessage.IdBookingTicketBroker,
                    Status = newBooking.Status,
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
              }
            }
            catch (Exception ex)
            {
              using (var scope = _serviceProvider.CreateScope())
              {
                var dbContext = scope.ServiceProvider.GetRequiredService<IDbContext>();

                var newBooking = new TrBookingTicket
                {
                  Id = new UuidV7().Value,
                  FullName = getBookingMessage.Name,
                  Email = getBookingMessage.Email,
                  CountTicket = getBookingMessage.CountTicket,
                  phone = getBookingMessage.Phone,
                  Status = BookingOrderStatus.Filed,
                  IdBookingTicketBroker = getBookingMessage.IdBookingTicketBroker,
                };
                await dbContext.InsertAsync(newBooking, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);

                #region MessageBroker
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
              }
            }

            //remove queue
            queue.DeleteMessage(message.MessageId, message.PopReceipt);
          }
        }
        catch (Exception ex)
        {

        }
      }
    }
  }
}

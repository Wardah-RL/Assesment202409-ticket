using Azure.Storage.Queues;
using DotnetApiTemplate.Core.Abstractions.Queue;
using DotnetApiTemplate.Core.Models.Queue;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Domain.Enums;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.Shared.Abstractions.Helpers;
using DotnetApiTemplate.Shared.Abstractions.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Infrastructure.Services.Queue
{
  public class BookingFeedbackQueueService : IGetQueue
  {
    private readonly QueueConfiguration _queueConfiguration;
    private readonly CancellationToken cancellationToken;
    private readonly IServiceProvider _serviceProvider;
    private readonly ISendQueue _emailQueue;

    public BookingFeedbackQueueService(QueueConfiguration queueConfiguration, IServiceProvider serviceProvider, ISendQueue emailQueue)
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

            var getBookingMessage = JsonConvert.DeserializeObject<BookingTicketFeedbackQueueRequest>(getMessage.Message);

            using (var scope = _serviceProvider.CreateScope())
            {
              var dbContext = scope.ServiceProvider.GetRequiredService<IDbContext>();

              var getBookingTicketBroker = await dbContext.Set<TrBookingTicketBroker>()
                             .Where(e => e.Id == getBookingMessage.IdBookingTicketBroker)
                             .FirstOrDefaultAsync(cancellationToken);

              if (getBookingTicketBroker != null)
              {
                dbContext.AttachEntity(getBookingTicketBroker);
                getBookingTicketBroker.Status = getBookingMessage.Status;
                await dbContext.SaveChangesAsync(cancellationToken);
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

using Azure.Core;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Core.Abstractions.Queue;
using DotnetApiTemplate.Core.Models.Queue;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.Shared.Abstractions.Helpers;
using DotnetApiTemplate.Shared.Abstractions.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ThirdParty.Json.LitJson;

namespace DotnetApiTemplate.Infrastructure.Services.Queue
{
    public class EventQueueService : IGetQueue
  {
    private readonly QueueConfiguration _queueConfiguration;
    private readonly CancellationToken cancellationToken;
    private readonly IServiceProvider _serviceProvider;

    public EventQueueService(QueueConfiguration queueConfiguration, IServiceProvider serviceProvider)
    {
      _queueConfiguration = queueConfiguration;
      _serviceProvider = serviceProvider;
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

            var getEventMessage = JsonConvert.DeserializeObject<EventMessageQueueRequest>(getMessage.Message);

            using (var scope = _serviceProvider.CreateScope())
            {
              var dbContext = scope.ServiceProvider.GetRequiredService<IDbContext>();

              var getEvent = await dbContext.Set<MsEvent>()
                             .Where(e => e.Id == getEventMessage.IdEvent)
                             .FirstOrDefaultAsync(cancellationToken);

              if (getEvent == null)
              {
                //create event
                var newEvent = new MsEvent
                {
                  Id = getEventMessage.IdEvent,
                  Name = getEventMessage.Name,
                  StartDate = getEventMessage.StartDate,
                  EndDate = getEventMessage.EndDate,
                  CountTicket = getEventMessage.CountTicket,
                  Price = getEventMessage.Price,
                  Location = getEventMessage.Location,
                };
                await dbContext.InsertAsync(newEvent, cancellationToken);
              }
              else
              {
                if (getMessage.Scenario == "DeleteEvent")
                {
                  //Delete event
                  dbContext.AttachEntity(getEvent);
                  getEvent.IsDeleted = true;
                }
                else
                {
                  //Update event
                  dbContext.AttachEntity(getEvent);
                  getEvent.Name = getEventMessage.Name;
                  getEvent.StartDate = getEventMessage.StartDate;
                  getEvent.EndDate = getEventMessage.EndDate;
                  getEvent.CountTicket = getEventMessage.CountTicket;
                  getEvent.Location = getEventMessage.Location;
                  getEvent.Price = getEventMessage.Price;
                  await dbContext.SaveChangesAsync(cancellationToken);
                }
              }

              await dbContext.SaveChangesAsync(cancellationToken);
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

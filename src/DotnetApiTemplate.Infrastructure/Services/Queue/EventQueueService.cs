using Azure.Core;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using DotnetApiTemplate.Core.Abstractions;
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
  public class EventQueueService : ReciverBaseQueueService
  {

    public EventQueueService(QueueConfiguration queueConfiguration, IServiceProvider serviceProvider) : base(queueConfiguration, serviceProvider)
    {
    }

    public async override Task<bool> QueueContent(IDbContext dbContext, CancellationToken cancellationToken, string scenario, SendQueueRequest message) 
    {
      try
      {
        var getEventMessage = JsonConvert.DeserializeObject<EventMessageQueueRequest>(message.Message);

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
          if (message.Scenario == "DeleteEvent")
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
        return true;
      }
      catch (Exception ex)
      {
        return false;
      }
    }
  }
}

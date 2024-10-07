using Azure.Storage.Queues;
using DotnetApiTemplate.Core.Models.Queue;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Domain.Enums;
using DotnetApiTemplate.Infrastructure.Services;
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

namespace DotnetApiTemplate.WebApi.Endpoints.Queue
{
  public class BookingFeedbackQueueService : ReciverBaseQueueService
  {
    public BookingFeedbackQueueService(QueueConfiguration queueConfiguration, IServiceProvider serviceProvider) : base(queueConfiguration, serviceProvider)
    {
    }

    public async override Task<bool> QueueContent(IDbContext dbContext, CancellationToken cancellationToken, string scenario, SendQueueRequest message)
    {
      try
      {
        var getBookingMessage = JsonConvert.DeserializeObject<BookingTicketFeedbackQueueRequest>(message.Message);

        var getBookingTicketBroker = await dbContext.Set<TrBookingTicket>()
                         .Where(e => e.Id == getBookingMessage.IdBookingTicket)
                         .FirstOrDefaultAsync(cancellationToken);

        if (getBookingTicketBroker != null)
        {
          dbContext.AttachEntity(getBookingTicketBroker);
          getBookingTicketBroker.Status = getBookingMessage.Status;
          getBookingTicketBroker.OrderCode = getBookingMessage.OrderCode;

          if (getBookingTicketBroker.Status == BookingOrderStatus.failed)
          {
            var getEvent = await dbContext.Set<MsEvent>()
                         .Where(e => e.Id == getBookingTicketBroker.IdEvent)
                         .FirstOrDefaultAsync(cancellationToken);

            dbContext.AttachEntity(getEvent);
            getEvent.CountTicket = getEvent.CountTicket+ getBookingTicketBroker.CountTicket;
          }
          await dbContext.SaveChangesAsync(cancellationToken);
        }
        return true;
      }
      catch (Exception ex)
      {

        return false;
      }
    }
  }
}

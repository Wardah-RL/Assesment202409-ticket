﻿using DotnetApiTemplate.Core.Abstractions.Queue;
using DotnetApiTemplate.Core.Models.Queue;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Contexts;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.WebApi.Endpoints.Event.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace DotnetApiTemplate.WebApi.Endpoints.Event
{
    public class DeleteEvent : BaseEndpointWithoutResponse<DeleteEventRequest>
  {
    private readonly IDbContext _dbContext;
    private readonly IStringLocalizer<DeleteEvent> _localizer;
    private readonly ISendQueue _emailQueue;
    public DeleteEvent(IDbContext dbContext,
        ISendQueue emailQueue,
        IStringLocalizer<DeleteEvent> localizer)
    {
      _dbContext = dbContext;
      _emailQueue = emailQueue;
      _localizer = localizer;
    }

    [HttpDelete("event/{idEvent}")]
    //[Authorize]
    [SwaggerOperation(
        Summary = "Delete event API",
        Description = "",
        OperationId = "Event.DeleteEvent",
        Tags = new[] { "Event" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync(
        DeleteEventRequest request,
        CancellationToken cancellationToken = new())
    {
      var geteventBroker = await _dbContext.Set<MsEventBroker>()
                      .Where(e => e.Id == request.IdEvent)
                      .FirstOrDefaultAsync(cancellationToken);

      if (geteventBroker == null)
        return BadRequest(Error.Create(string.Format(_localizer["event-not-found"], request.IdEvent)));

      _dbContext.AttachEntity(geteventBroker);
      geteventBroker.IsDeleted = true;
      await _dbContext.SaveChangesAsync(cancellationToken);

      #region MessageBroker
      EventMessageQueueRequest getEventBrokerMessage = new EventMessageQueueRequest
      {
        IdEvent = geteventBroker.Id,
        CountTicket = geteventBroker.CountTicket,
        EndDate = geteventBroker.EndDate,
        StartDate = geteventBroker.StartDate,
        Location = geteventBroker.Location,
        Name = geteventBroker.Name,
        Price = geteventBroker.Price,
      };
      
      SendQueueRequest _paramQueue = new SendQueueRequest
      {
        Message = JsonSerializer.Serialize(getEventBrokerMessage),
        Scenario = "DeleteEvent",
        QueueName = "event"
      };

      _emailQueue.Execute(_paramQueue);
      #endregion

      return NoContent();
    }
  }
}

using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Core.Models;
using DotnetApiTemplate.Core.Models.Queue;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.Shared.Abstractions.Encryption;
using DotnetApiTemplate.Shared.Abstractions.Helpers;
using DotnetApiTemplate.WebApi.Common;
using DotnetApiTemplate.WebApi.Endpoints.Event.Request;
using DotnetApiTemplate.WebApi.Endpoints.Event.Validator;
using DotnetApiTemplate.WebApi.Endpoints.UserManagement.Requests;
using DotnetApiTemplate.WebApi.Endpoints.UserManagement.Scopes;
using DotnetApiTemplate.WebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Text.Json;

namespace DotnetApiTemplate.WebApi.Endpoints.Event
{
    public class CreateEvent : BaseEndpointWithoutResponse<CreateEventRequest>
  {
    private readonly IDbContext _dbContext;
    private readonly IStringLocalizer<CreateEvent> _localizer;
    private readonly ISendQueue _emailQueue;
    public CreateEvent(IDbContext dbContext,
        ISendQueue emailQueue,
        IStringLocalizer<CreateEvent> localizer)
    {
      _dbContext = dbContext;
      _emailQueue = emailQueue;
      _localizer = localizer;
    }

    [HttpPost("event")]
    //[Authorize]
    [SwaggerOperation(
        Summary = "Create event API",
        Description = "",
        OperationId = "Event.CreateEvent",
        Tags = new[] { "Event" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync(CreateEventRequest request,
        CancellationToken cancellationToken = new())
    {
      var validator = new CreateEventValidator();
      var validationResult = await validator.ValidateAsync(request, cancellationToken);
      if (!validationResult.IsValid)
        return BadRequest(Error.Create(_localizer["invalid-parameter"], validationResult.Construct()));

      var newEventBroker = new MsEvent
      {
        Id = new UuidV7().Value,
        Name = request.Name,
        StartDate = request.StartDate,
        EndDate = request.EndDate,
        CountTicket = request.CountTicket,
        Location = request.Location,
        Price = request.Price
      };
      await _dbContext.InsertAsync(newEventBroker, cancellationToken);
      await _dbContext.SaveChangesAsync(cancellationToken);

      #region MessageBroker
      var getEventBroker = await _dbContext.Set<MsEvent>()
               .Where(e => e.Id == newEventBroker.Id)
               .Select(e => new EventMessageQueueRequest
               {
                 IdEvent = e.Id,
                 CountTicket = e.CountTicket,
                 EndDate = e.EndDate,
                 StartDate = e.StartDate,
                 Location = e.Location,
                 Name = e.Name,
                 Price = e.Price,
               })
               .FirstOrDefaultAsync(cancellationToken);

      SendQueueRequest _paramQueue = new SendQueueRequest
      {
        Message = JsonSerializer.Serialize(getEventBroker),
        Scenario = "CreateEvent",
        QueueName = "event"
      };

      _emailQueue.Execute(_paramQueue);
      #endregion

      return NoContent();
    }
  }
}

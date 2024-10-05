using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Core.Models.Queue;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.Shared.Abstractions.Helpers;
using DotnetApiTemplate.WebApi.Common;
using DotnetApiTemplate.WebApi.Endpoints.Event.Request;
using DotnetApiTemplate.WebApi.Endpoints.Event.Validator;
using DotnetApiTemplate.WebApi.Endpoints.UserManagement.Scopes;
using DotnetApiTemplate.WebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace DotnetApiTemplate.WebApi.Endpoints.Event
{
    public class UpdateEvent : BaseEndpointWithoutResponse<UpdateEventRequest>
  {
    private readonly IDbContext _dbContext;
    private readonly IStringLocalizer<UpdateEventRequest> _localizer;
    private readonly ISendQueue _emailQueue;
    public UpdateEvent(IDbContext dbContext,
        ISendQueue emailQueue,
        IStringLocalizer<UpdateEventRequest> localizer)
    {
      _dbContext = dbContext;
      _emailQueue = emailQueue;
      _localizer = localizer;
    }

    [HttpPut("event")]
    //[Authorize]
    [SwaggerOperation(
        Summary = "Update event API",
        Description = "",
        OperationId = "Event.UpdateEvent",
        Tags = new[] { "Event" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync(UpdateEventRequest request,
        CancellationToken cancellationToken = new())
    {
      var validator = new UpdateEventValidator();
      var validationResult = await validator.ValidateAsync(request, cancellationToken);
      if (!validationResult.IsValid)
        return BadRequest(Error.Create(_localizer["invalid-parameter"], validationResult.Construct()));

      var getEventBroker = await _dbContext.Set<MsEventBroker>()
                              .Where(e => e.Id == request.IdEvent)
                              .FirstOrDefaultAsync(cancellationToken);

      if (getEventBroker == null)
        return BadRequest(Error.Create(string.Format(_localizer["event-not-found"], request.Name)));

      _dbContext.AttachEntity(getEventBroker);
      getEventBroker.Name = request.Name;
      getEventBroker.StartDate = request.StartDate;
      getEventBroker.EndDate = request.EndDate;
      getEventBroker.CountTicket = request.CountTicket;
      getEventBroker.Location = request.Location;
      getEventBroker.Price = request.Price;

      await _dbContext.SaveChangesAsync(cancellationToken);

      #region MessageBroker
      var getEventBrokerMessage = await _dbContext.Set<MsEventBroker>()
                                  .Where(e => e.Id == getEventBroker.Id)
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
        Message = JsonSerializer.Serialize(getEventBrokerMessage),
        Scenario = "UpdateEvent",
        QueueName = "event"
      };

      _emailQueue.Execute(_paramQueue);
      #endregion

      return NoContent();
    }
  }
}

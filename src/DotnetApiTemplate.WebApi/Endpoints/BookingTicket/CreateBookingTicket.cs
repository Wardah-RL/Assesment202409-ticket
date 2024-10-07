using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Core.Models;
using DotnetApiTemplate.Core.Models.Queue;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Domain.Enums;
using DotnetApiTemplate.Shared.Abstractions.Clock;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.Shared.Abstractions.Helpers;
using DotnetApiTemplate.WebApi.Endpoints.BookingTicket.Request;
using DotnetApiTemplate.WebApi.Endpoints.BookingTicket.Validator;
using DotnetApiTemplate.WebApi.Endpoints.Event.Request;
using DotnetApiTemplate.WebApi.Endpoints.Event.Validator;
using DotnetApiTemplate.WebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace DotnetApiTemplate.WebApi.Endpoints.BookingTicket
{
    public class CreateBookingTicket : BaseEndpointWithoutResponse<CreateBookingTicketRequest>
  {
    private readonly IDbContext _dbContext;
    private readonly IStringLocalizer<CreateBookingTicket> _localizer;
    private readonly ISendQueue _emailQueue;
    private readonly IClock _clock;

    public CreateBookingTicket(IDbContext dbContext,
        ISendQueue emailQueue,
        IStringLocalizer<CreateBookingTicket> localizer, IClock clock)
    {
      _dbContext = dbContext;
      _emailQueue = emailQueue;
      _localizer = localizer;
      _clock = clock;
    }

    [HttpPost("bookingTicket")]
    //[Authorize]
    [SwaggerOperation(
        Summary = "Create booking ticket API",
        Description = "",
        OperationId = "BookingTicket.CreateBookingTicket",
        Tags = new[] { "BookingTicket" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync(CreateBookingTicketRequest request,
        CancellationToken cancellationToken = new())
    {
      var validator = new CreateBookingTicketValidator();
      var validationResult = await validator.ValidateAsync(request, cancellationToken);
      if (!validationResult.IsValid)
        return BadRequest(Error.Create(_localizer["invalid-parameter"], validationResult.Construct()));

      var getEvent = await _dbContext.Set<MsEvent>()
                            .Where(e => e.Id == request.IdEvent)
                            .FirstOrDefaultAsync(cancellationToken);

      if (getEvent != null)
      {
        if (getEvent.CountTicket == 0)
          return BadRequest(Error.Create(_localizer["ticket-booking-sold"], validationResult.Construct()));

        if (getEvent.CountTicket < request.CountTicket)
          return BadRequest(Error.Create(_localizer["ticket-available"] +" "+ getEvent.CountTicket + " " + _localizer["ticket"], validationResult.Construct()));

        if(getEvent.StartDate.Date<=request.DateEvent.Date && getEvent.EndDate.Date >= request.DateEvent.Date)
        {

        }
        else
          return BadRequest(Error.Create(_localizer["event-date-wrong"], validationResult.Construct()));

        if (_clock.CurrentDate().Date >= getEvent.EndDate.Date)
          return BadRequest(Error.Create(_localizer["event-out-of-date"], validationResult.Construct()));


        _dbContext.AttachEntity(getEvent);
        getEvent.CountTicket = getEvent.CountTicket - request.CountTicket;
      }

      var newBooking = new TrBookingTicket
      {
        Id = new UuidV7().Value,
        IdEvent = request.IdEvent,
        IdUser = request.IdUser,
        CountTicket = request.CountTicket,
        Status = BookingOrderStatus.Processed,
        DateEvent = request.DateEvent,
      };

      await _dbContext.InsertAsync(newBooking, cancellationToken);
      await _dbContext.SaveChangesAsync(cancellationToken);

      #region MessageBroker
      var getBooking = await _dbContext.Set<TrBookingTicket>()
               .Include(e=>e.User)
               .Where(e => e.Id == newBooking.Id)
               .Select(e => new BookingTicketQueueRequest
               {
                 IdEvent = e.IdEvent,
                 CountTicket = e.CountTicket,
                 Email = e.User.Email,
                 Name = e.User.FullName,
                 Phone = e.User.Phone,
                 IdBookingTicketBroker = e.Id,
                 DateEvent = e.DateEvent,
               })
               .FirstOrDefaultAsync(cancellationToken);

      SendQueueRequest _paramQueue = new SendQueueRequest
      {
        Message = JsonSerializer.Serialize(getBooking),
        Scenario = "BookingTicket",
        QueueName = "bookingticket"
      };

      _emailQueue.Execute(_paramQueue);
      #endregion

      return NoContent();
    }
  }
}

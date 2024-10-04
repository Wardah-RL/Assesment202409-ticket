using DotnetApiTemplate.Core.Abstractions.Queue;
using DotnetApiTemplate.Core.Models;
using DotnetApiTemplate.Core.Models.Queue;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Domain.Enums;
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
    public CreateBookingTicket(IDbContext dbContext,
        ISendQueue emailQueue,
        IStringLocalizer<CreateBookingTicket> localizer)
    {
      _dbContext = dbContext;
      _emailQueue = emailQueue;
      _localizer = localizer;
    }

    [HttpPost("booking")]
    //[Authorize]
    [SwaggerOperation(
        Summary = "Create booking API",
        Description = "",
        OperationId = "Booking.CreateBooking",
        Tags = new[] { "Booking" })
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

      var getEvent = await _dbContext.Set<MsEventBroker>()
                            .Where(e => e.Id == request.IdEvent)
                            .FirstOrDefaultAsync(cancellationToken);

      if (getEvent != null)
      {
        _dbContext.AttachEntity(getEvent);
        getEvent.CountTicket = getEvent.CountTicket - request.CountTicket;
      }

      var newBooking = new TrBookingTicketBroker
      {
        Id = new UuidV7().Value,
        IdEventBroker = request.IdEvent,
        IdUser = request.IdUser,
        CountTicket = request.CountTicket,
        Status = BookingOrderStatus.Process,
      };
      await _dbContext.InsertAsync(newBooking, cancellationToken);
      await _dbContext.SaveChangesAsync(cancellationToken);

      #region MessageBroker
      var getBooking = await _dbContext.Set<TrBookingTicketBroker>()
               .Include(e=>e.User)
               .Where(e => e.Id == newBooking.Id)
               .Select(e => new BookingTicketQueueRequest
               {
                 IdEvent = e.IdEventBroker,
                 CountTicket = e.CountTicket,
                 Email = e.User.Email,
                 Name = e.User.FullName,
                 Phone = e.User.Phone,
                 IdBookingTicketBroker = e.Id
               })
               .FirstOrDefaultAsync(cancellationToken);

      ////SendQueueRequest _paramQueue = new SendQueueRequest
      ////{
      ////  Message = JsonSerializer.Serialize(getBooking),
      ////  Scenario = "BookingTicket",
      ////  QueueName = "bookingticket"
      ////};

      //_emailQueue.Execute(_paramQueue);
      #endregion

      return NoContent();
    }
  }
}

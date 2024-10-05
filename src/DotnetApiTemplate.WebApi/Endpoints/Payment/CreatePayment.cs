using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Core.Models.Queue;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.Shared.Abstractions.Helpers;
using DotnetApiTemplate.WebApi.Endpoints.Event.Request;
using DotnetApiTemplate.WebApi.Endpoints.Event.Validator;
using DotnetApiTemplate.WebApi.Endpoints.Payment.Request;
using DotnetApiTemplate.WebApi.Endpoints.Payment.Validator;
using DotnetApiTemplate.WebApi.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace DotnetApiTemplate.WebApi.Endpoints.Payment
{
    public class CreatePayment : BaseEndpointWithoutResponse<CreatePaymentRequest>
  {
    private readonly IDbContext _dbContext;
    private readonly IStringLocalizer<CreatePayment> _localizer;
    private readonly ISendQueue _emailQueue;
    public CreatePayment(IDbContext dbContext,
        ISendQueue emailQueue,
        IStringLocalizer<CreatePayment> localizer)
    {
      _dbContext = dbContext;
      _emailQueue = emailQueue;
      _localizer = localizer;
    }

    [HttpPost("payment")]
    //[Authorize]
    [SwaggerOperation(
        Summary = "Create payment API",
        Description = "",
        OperationId = "Payment.CreateEPayment",
        Tags = new[] { "Payment" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync(CreatePaymentRequest request,
        CancellationToken cancellationToken = new())
    {
      var validator = new CreatePaymentValidator();
      var validationResult = await validator.ValidateAsync(request, cancellationToken);
      if (!validationResult.IsValid)
        return BadRequest(Error.Create(_localizer["invalid-parameter"], validationResult.Construct()));

      var bookingTicket = await _dbContext.Set<TrBookingTicketBroker>()
                                    .Include(e => e.EventBroker)
                                    .Where(e => e.Id == request.IdBookingTicketBroker)
                                    .FirstOrDefaultAsync(cancellationToken);

      if(bookingTicket == null)
        return BadRequest(Error.Create(_localizer["booking-ticket-not-found"], validationResult.Construct()));

      var total = bookingTicket.CountTicket * bookingTicket.EventBroker.Price;

      if(total !=request.TotalPayment)
        return BadRequest(Error.Create(_localizer["total-payment-is-not-same"], validationResult.Construct()));

      var newEventBroker = new TrPaymentBroker
      {
        Id = new UuidV7().Value,
        IdBookingTicketBroker = request.IdBookingTicketBroker,
        IdBank = request.IdBank,
        TotalPayment = request.TotalPayment,
        NamaPengirim = request.NamaPengirim,
        NoRekening = request.NoRekening,
      };

      await _dbContext.InsertAsync(newEventBroker, cancellationToken);
      await _dbContext.SaveChangesAsync(cancellationToken);

      #region MessageBroker
      var getEventBroker = await _dbContext.Set<TrPaymentBroker>()
               .Where(e => e.Id == newEventBroker.Id)
               .Select(e => new PaymentMessageQueueRequest
               {
                 IdBookingTicket = e.IdBookingTicketBroker,
                 Bank = e.Bank.Name,
                 NamaPengirim = e.NamaPengirim,
                 NoRekening = e.NoRekening,
                 TotalPayment = e.TotalPayment,
                 OrderCode = e.BookingTicketBroker.OrderCode.Value
               })
               .FirstOrDefaultAsync(cancellationToken);

      SendQueueRequest _paramQueue = new SendQueueRequest
      {
        Message = JsonSerializer.Serialize(getEventBroker),
        Scenario = "CreatePayment",
        QueueName = "payment"
      };

      _emailQueue.Execute(_paramQueue);
      #endregion

      return NoContent();
    }
  }
}

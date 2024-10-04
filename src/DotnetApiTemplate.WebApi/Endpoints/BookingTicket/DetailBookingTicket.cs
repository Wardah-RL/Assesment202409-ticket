using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.WebApi.Contracts.Responses;
using DotnetApiTemplate.WebApi.Endpoints.BookingTicket.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace DotnetApiTemplate.WebApi.Endpoints.BookingTicket
{

  public class DetailBookingTicket : BaseEndpoint<DetailBookingTicketRequest, DetailBookingTicketRespone>
  {
    private readonly IDbContext _dbContext;
    private readonly IStringLocalizer<DetailBookingTicket> _localizer;

    public DetailBookingTicket(IDbContext dbContext,
        IStringLocalizer<DetailBookingTicket> localizer)
    {
      _dbContext = dbContext;
      _localizer = localizer;
    }

    [HttpGet("bookingTicket/{idBookingTicket}")]
    //[Authorize]
    [SwaggerOperation(
        Summary = "Detail booking ticket API",
        Description = "",
        OperationId = "BookingTicket.DetailBookingTicket",
        Tags = new[] { "BookingTicket" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<DetailBookingTicketRespone>> HandleAsync([FromRoute] DetailBookingTicketRequest request,
        CancellationToken cancellationToken = new())
    {
      ;var bookingTicket = await _dbContext.Set<TrBookingTicketBroker>()
                              .Include(e => e.EventBroker)
                              .Where(e => e.Id == request.IdBookingTicket)
                              .FirstOrDefaultAsync(cancellationToken);

      if (bookingTicket == null)
        return BadRequest(Error.Create(string.Format(_localizer["booking-ticket-not-found"], request.IdBookingTicket)));

      var response = new DetailBookingTicketRespone
      {
        IdBookingTicket = bookingTicket.Id,
        NameEvent = bookingTicket.EventBroker.Name,
        CountTicket = bookingTicket.CountTicket,
        Status = bookingTicket.Status.ToString(),
        DateEvent = bookingTicket.DateEvent,
      };

      return response;
    }
  }
}

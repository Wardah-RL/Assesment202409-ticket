using Microsoft.AspNetCore.Mvc;

namespace DotnetApiTemplate.WebApi.Endpoints.BookingTicket.Request
{
  public class DetailBookingTicketRequest
  {
    [FromRoute(Name = "idBookingTicket")] public Guid IdBookingTicket { get; set; }
  }
}

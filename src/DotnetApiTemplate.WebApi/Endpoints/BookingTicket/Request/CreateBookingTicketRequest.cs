namespace DotnetApiTemplate.WebApi.Endpoints.BookingTicket.Request
{
  public class CreateBookingTicketRequest
  {
    public Guid IdEvent { get; set; }
    public Guid IdUser { get; set; }
    public int CountTicket { get; set; }
    public DateTime DateEvent { get; set; }
  }
}

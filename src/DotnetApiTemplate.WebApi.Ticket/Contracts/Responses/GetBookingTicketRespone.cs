namespace DotnetApiTemplate.WebApi.Contracts.Responses
{
  public class GetBookingTicketRespone
  {
    public Guid IdBookingTicket { get; set; }
    public string NameEvent { get; set; } =  string.Empty;
    public int CountTicket { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime DateEvent { get; set; }
    public int Price {  get; set; }
    public int Total {  get; set; }
  }
}

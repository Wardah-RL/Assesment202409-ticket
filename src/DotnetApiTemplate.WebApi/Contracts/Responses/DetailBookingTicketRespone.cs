﻿namespace DotnetApiTemplate.WebApi.Contracts.Responses
{
  public class DetailBookingTicketRespone
  {
    public Guid IdBookingTicket { get; set; }
    public string NameEvent { get; set; } = string.Empty;
    public int CountTicket { get; set; }
    public string Status { get; set; } = string.Empty;
  }
}

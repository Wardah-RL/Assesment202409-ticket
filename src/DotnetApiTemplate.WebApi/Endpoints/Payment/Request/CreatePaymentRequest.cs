using DotnetApiTemplate.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetApiTemplate.WebApi.Endpoints.Payment.Request
{
  public class CreatePaymentRequest
  {
    public Guid IdBookingTicket { get; set; }
    public Guid IdBank { get; set; }
    public int TotalPayment { get; set; }
    public string NamaPengirim { get; set; } = string.Empty;
    public string NoRekening { get; set; } = string.Empty;
  }
}

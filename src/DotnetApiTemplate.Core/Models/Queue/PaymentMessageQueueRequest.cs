using DotnetApiTemplate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Core.Models.Queue
{
  public class PaymentMessageQueueRequest
  {
    public Guid IdBookingTicket { get; set; }
    public int TotalPayment { get; set; }
    public string NamaPengirim { get; set; } = string.Empty;
    public string Bank { get; set; } = string.Empty;
    public string NoRekening { get; set; } = string.Empty;
    public Guid OrderCode { get; set; }
  }
}

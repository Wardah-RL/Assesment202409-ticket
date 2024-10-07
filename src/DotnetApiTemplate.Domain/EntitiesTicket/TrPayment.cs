using DotnetApiTemplate.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Domain.EntitiesTicket
{
  public class TrPayment : BaseEntity
  {
    [ForeignKey(nameof(BookingTicket))]
    public Guid IdBookingTicket { get; set; }
    public TrBookingTicket? BookingTicket { get; set; }
    public int TotalPayment { get; set; }
    public string NamaPengirim { get; set; } = string.Empty;
    public string Bank { get; set; } = string.Empty;
    public string NoRekening { get; set; } = string.Empty;
  }
}


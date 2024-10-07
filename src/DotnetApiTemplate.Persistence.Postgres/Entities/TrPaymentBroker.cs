using DotnetApiTemplate.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Persistence.Postgres.Entities
{
  public class TrPaymentBroker : BaseEntity
  {
    [ForeignKey(nameof(BookingTicketBroker))]
    public Guid IdBookingTicketBroker { get; set; }
    public TrBookingTicketBroker? BookingTicketBroker { get; set; }

    [ForeignKey(nameof(Bank))]
    public Guid IdBank { get; set; }
    public MsBank? Bank { get; set; }
    public int TotalPayment { get; set; }
    public string NamaPengirim { get; set; } = string.Empty;
    public string NoRekening { get; set; } = string.Empty;
  }
}

using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Persistence.Postgres.Ticket.Entities;
using DotnetApiTemplate.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Persistence.Postgres.Ticket.Configurations
{
  public class TrBookingTicketConfiguration : BaseEntityConfiguration<TrBookingTicket>
  {
    protected override void EntityConfiguration(EntityTypeBuilder<TrBookingTicket> builder)
    {
      builder.Property(e => e.FullName).HasMaxLength(100);
      builder.Property(e => e.Email).HasMaxLength(50);
      builder.Property(e => e.phone).HasMaxLength(20);

    }
  }
}

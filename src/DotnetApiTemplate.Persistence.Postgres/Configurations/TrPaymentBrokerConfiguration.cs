using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Persistence.Postgres.Configurations
{
  public class TrPaymentBrokerConfiguration : BaseEntityConfiguration<TrPayment>
  {
    protected override void EntityConfiguration(EntityTypeBuilder<TrPayment> builder)
    {
      builder.Property(e => e.NamaPengirim).HasMaxLength(256);
      builder.Property(e => e.NoRekening).HasMaxLength(50);
    }
  }
}

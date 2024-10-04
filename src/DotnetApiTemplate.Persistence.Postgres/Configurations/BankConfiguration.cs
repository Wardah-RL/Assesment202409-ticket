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
  public class BankConfiguration : BaseEntityConfiguration<MsBank>
  {
    protected override void EntityConfiguration(EntityTypeBuilder<MsBank> builder)
    {
      builder.Property(e => e.Name).HasMaxLength(100);
    }
  }
}

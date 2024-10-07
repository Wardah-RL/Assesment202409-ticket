using DotnetApiTemplate.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Persistence.Postgres.Entities
{
  public class MsBank : BaseEntity
  {
    public string Name { get; set; } = null!;
  }
}

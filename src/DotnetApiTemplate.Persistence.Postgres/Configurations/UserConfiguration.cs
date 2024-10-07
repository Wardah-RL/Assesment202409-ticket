﻿using DotnetApiTemplate.Persistence.Postgres.Entities;
using DotnetApiTemplate.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetApiTemplate.Persistence.Postgres.Configurations;

public class UserConfiguration : BaseEntityConfiguration<User>
{
    protected override void EntityConfiguration(EntityTypeBuilder<User> builder)
    {
        builder.Property(e => e.Username)
            .HasMaxLength(256);
        builder.Property(e => e.NormalizedUsername)
            .HasMaxLength(256);
        builder.Property(e => e.Salt)
            .HasMaxLength(512);
        builder.Property(e => e.Password)
            .HasMaxLength(512);
        builder.Property(e => e.FullName).HasMaxLength(512);
        builder.Property(e => e.Email)
            .HasMaxLength(256);
    }
}
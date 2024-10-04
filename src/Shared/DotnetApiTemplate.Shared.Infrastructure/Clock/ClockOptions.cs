﻿namespace DotnetApiTemplate.Shared.Infrastructure.Clock;

public sealed class ClockOptions
{
    public ClockOptions()
    {
        Hours = 7;
    }

    /// <summary>
    /// Default value is 7
    /// </summary>
    public int Hours { get; set; }
}
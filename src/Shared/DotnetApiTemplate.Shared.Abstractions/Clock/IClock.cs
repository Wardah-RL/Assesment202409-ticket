﻿namespace DotnetApiTemplate.Shared.Abstractions.Clock;

public interface IClock
{
    DateTime CurrentDate();

    DateTime CurrentServerDate();
}
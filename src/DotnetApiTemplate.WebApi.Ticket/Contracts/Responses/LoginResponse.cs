﻿using DotnetApiTemplate.Domain.Entities;

namespace DotnetApiTemplate.WebApi.Ticket.Contracts.Responses;

public class LoginResponse
{
    public LoginResponse()
    {
    }

    public LoginResponse(User user)
    {
        UserId = user.Id;
    }

    public Guid UserId { get; set; }
    public long Expiry { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}
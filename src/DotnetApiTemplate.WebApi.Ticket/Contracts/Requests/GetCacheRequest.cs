using Microsoft.AspNetCore.Mvc;

namespace DotnetApiTemplate.WebApi.Ticket.Contracts.Requests;

public class GetCacheRequest
{
    [FromRoute(Name = "key")] public string Key { get; set; } = null!;
}
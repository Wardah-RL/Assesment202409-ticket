﻿using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Contexts;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.Shared.Abstractions.Queries;
using DotnetApiTemplate.Shared.Infrastructure;
using DotnetApiTemplate.WebApi.Contracts.Responses;
using DotnetApiTemplate.WebApi.Endpoints.Event.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq.Dynamic.Core;

namespace DotnetApiTemplate.WebApi.Endpoints.BookingTicket
{
  public class GetBookingTicket : BaseEndpoint<GetAllMenuPaginatedRequest, PagedList<GetBookingTicketRespone>>
  {
    private readonly IDbContext _dbContext;
    private readonly IStringLocalizer<GetBookingTicket> _localizer;
    private readonly IContext? _context;

    public GetBookingTicket(IDbContext dbContext,
        IStringLocalizer<GetBookingTicket> localizer,
        IContext? context)
    {
      _dbContext = dbContext;
      _localizer = localizer;
      _context = context;
    }

    [HttpGet("bookingTicket")]
    [Authorize]
    [SwaggerOperation(
       Summary = "Get booking ticket API",
       Description = "",
       OperationId = "BookingTicket.GetBooking",
       Tags = new[] { "BookingTicket" })
   ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<PagedList<GetBookingTicketRespone>>> HandleAsync([FromQuery] GetAllMenuPaginatedRequest request,
        CancellationToken cancellationToken = new())
    {
      var queryable = _dbContext.Set<TrBookingTicketBroker>()
                      .Include(e=>e.EventBroker)
                      .Where(e=>e.IdUser== _context!.Identity.Id)
                      .AsQueryable();

      var totalRows = await queryable.CountAsync(cancellationToken);

      if (!string.IsNullOrWhiteSpace(request.Search) && request.Search.Length > 2)
        queryable = queryable.Where(e => e.EventBroker.Name.ToLower().Contains(request.Search.ToLower()));

      if (!string.IsNullOrWhiteSpace(request.OrderBy) && !string.IsNullOrWhiteSpace(request.OrderType))
        queryable = queryable.OrderBy(request.OrderType, request.OrderBy);
      else
        queryable = queryable.OrderBy("EventBroker.Name", "ASC");

      var listBookingTicket = queryable
          .Select(e => new 
          {
            BookingTicketId = e.Id,
            NameEvent = e.EventBroker.Name,
            Status = e.Status.ToString(),
            CountTicket = e.CountTicket,
            DateEvent = e.DateEvent,
            Price = e.EventBroker.Price
          })
          .Skip(request.CalculateSkip())
          .ToList();

      var getBookingTicket = listBookingTicket
         .Select(e => new GetBookingTicketRespone
         {
           IdBookingTicket = e.BookingTicketId,
           NameEvent = e.NameEvent,
           CountTicket = e.CountTicket,
           Status = e.Status.ToString(),
           DateEvent = e.DateEvent,
           Price = e.Price,
           Total = e.Price*e.CountTicket
         })
         .ToList();

      var totalRowFilter = getBookingTicket.Count;

      var response = new PagedList<GetBookingTicketRespone>(getBookingTicket, request.Page, request.Size);

      return response;
    }
  }
}

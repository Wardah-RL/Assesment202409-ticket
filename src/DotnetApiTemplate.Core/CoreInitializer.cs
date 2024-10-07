using DotnetApiTemplate.Domain;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Domain.Extensions;
using DotnetApiTemplate.Shared.Abstractions.Clock;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.Shared.Abstractions.Encryption;
using DotnetApiTemplate.Shared.Abstractions.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DotnetApiTemplate.Core;

public class CoreInitializer : IInitializer
{
  private readonly IDbContext _dbContext;
  private readonly ISalter _salter;
  private readonly IRng _rng;
  private readonly IClock _clock;

  public CoreInitializer(IDbContext dbContext, ISalter salter, IRng rng, IClock clock)
  {
    _dbContext = dbContext;
    _salter = salter;
    _rng = rng;
    _clock = clock;
  }

  public async Task ExecuteAsync(CancellationToken cancellationToken)
  {
    await AddSuperAdministratorRoleAsync(cancellationToken);

    await AddSuperAdministratorAsync(cancellationToken);
    await AddBank(cancellationToken);
    await AddTempalte(cancellationToken);

    await _dbContext.SaveChangesAsync(cancellationToken);
  }

  private async Task AddSuperAdministratorRoleAsync(CancellationToken cancellationToken)
  {
    if (await _dbContext.Set<Role>().AnyAsync(e => e.Id == Guid.Empty,
            cancellationToken: cancellationToken))
      return;

    var role = new Role
    {
      IsDefault = true,
      Id = RoleExtensions.SuperAdministratorId,
      Name = RoleExtensions.SuperAdministratorName,
      Code = RoleExtensions.Slug(Guid.Empty, RoleExtensions.SuperAdministratorName),
      Description = "Default role to the application"
    };

    await _dbContext.InsertAsync(role, cancellationToken);
  }

  private async Task AddSuperAdministratorAsync(CancellationToken cancellationToken)
  {
    if (await _dbContext.Set<User>().AnyAsync(e => e.Id == DefaultUser.SuperAdministratorId,
            cancellationToken: cancellationToken))
      return;

    var user = DefaultUser.SuperAdministrator(_rng, _salter, _clock);

    await _dbContext.InsertAsync(user, cancellationToken);
  }

  private async Task AddBank(CancellationToken cancellationToken)
  {
    if (await _dbContext.Set<MsBank>().AnyAsync(e => e.Id == Guid.Empty,
            cancellationToken: cancellationToken))
      return;

    var listBank = await _dbContext.Set<MsBank>().ToListAsync(cancellationToken);

    var allBank = new List<MsBank>();

    var bank = new List<MsBank>
    {
        new() { Id = new UuidV7().Value, Name = "BCA"},
        new() { Id = new UuidV7().Value, Name = "Mandiri"},
        new() { Id = new UuidV7().Value, Name = "BRI"},
    };

    foreach (var item in bank)
    {
      var dataBank = listBank.FirstOrDefault(x => x.Name == item.Name);

      if (dataBank == null)
      {
        item.Id = new UuidV7().Value;
        allBank.Add(item);
        _dbContext.Insert(item);
      }
    }

    await _dbContext.SaveChangesAsync(cancellationToken);
  }

  private async Task AddTempalte(CancellationToken cancellationToken)
  {
    if (await _dbContext.Set<MsTemplate>().AnyAsync(e => e.Id == Guid.Empty,
            cancellationToken: cancellationToken))
      return;

    var listTemplate = await _dbContext.Set<MsTemplate>().ToListAsync(cancellationToken);

    var allTemplate = new List<MsTemplate>();

    var template = new List<MsTemplate>
    {
        new() 
        { 
          Id = new UuidV7().Value, 
          Code = "T001", 
          Subject = "Ordering success", 
          HTMLContent = @"<html>
                            <body>
                              <p>Dear Mr {{recepientName}},</p>
                              <p>Your ticket has been successfully booked.</p>
                              <br>
                              <table>
                                <tr>
                                  <td>Event Name</td>
                                  <td>:</td>
                                  <td>{{event}}</td>
                                </tr>
                                <tr>
                                  <td>Date</td>
                                  <td>:</td>
                                  <td>{{date}}</td>
                                </tr>
                                <tr>
                                  <td>Count Ticket</td>
                                  <td>:</td>
                                  <td>{{countTicket}}</td>
                                </tr>
                                <tr>
                                  <td>Price</td>
                                  <td>:</td>
                                  <td>{{price}}</td>
                                </tr>
                                <tr>
                                  <td>Total</td>
                                  <td>:</td>
                                  <td>{{total}}</td>
                                </tr>
                              </table>
                            </body>
                          </html>", 
          TextContent = "", 
          IsHtml = true
        },
        new()
        {
          Id = new UuidV7().Value,
          Code = "T002",
          Subject = "Ordering failed",
          HTMLContent = @"<html>
                            <body>
                              <p>Dear Mr {{recepientName}},</p>
                              <p>Your ticket has failed to be booked.</p>
                              <br>
                              <table>
                                <tr>
                                  <td>Event Name</td>
                                  <td>:</td>
                                  <td>{{event}}</td>
                                </tr>
                                <tr>
                                  <td>Date</td>
                                  <td>:</td>
                                  <td>{{date}}</td>
                                </tr>
                                <tr>
                                  <td>Count Ticket</td>
                                  <td>:</td>
                                  <td>{{countTicket}}</td>
                                </tr>
                                <tr>
                                  <td>Price</td>
                                  <td>:</td>
                                  <td>{{price}}</td>
                                </tr>
                                <tr>
                                  <td>Total</td>
                                  <td>:</td>
                                  <td>{{total}}</td>
                                </tr>
                              </table>
                            </body>
                          </html>",
          TextContent = "",
          IsHtml = true
        },
    };

    foreach (var item in template)
    {
      var dataTemplate = listTemplate.FirstOrDefault(x => x.Code == item.Code);

      if (dataTemplate == null)
      {
        item.Id = new UuidV7().Value;
        allTemplate.Add(item);
        _dbContext.Insert(item);
      }
    }

    await _dbContext.SaveChangesAsync(cancellationToken);
  }
}
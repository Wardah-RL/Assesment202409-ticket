using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DotnetApiTemplate.Infrastructure.Ticket.Services;

public class RoleService : IRoleService
{
    private readonly IDbContext _dbContext;

    public RoleService(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<Role> GetBaseQuery()
        => _dbContext.Set<Role>()
            .Where(e => e.IsDefault == false)
            .AsQueryable();

    public Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<Role?> CreateAsync(Role entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.InsertAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var role = await GetByExpressionAsync(
            e => e.Id == id,
            e => new Role
            {
                Id = e.Id
            }, cancellationToken);

        if (role is null)
            throw new Exception("Data role not found");

        _dbContext.AttachEntity(role);

        role.SetToDeleted();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Role?> GetByExpressionAsync(
        Expression<Func<Role, bool>> predicate,
        Expression<Func<Role, Role>> projection,
        CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(predicate)
            .Select(projection)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<Role?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(e => e.Code == code)
            .FirstOrDefaultAsync(cancellationToken);
}
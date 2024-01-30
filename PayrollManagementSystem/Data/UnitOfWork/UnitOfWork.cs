using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using PayrollManagementSystem.Context;

namespace PayrollManagementSystem.Data.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UnitOfWork(PayrollDbContext context, IHttpContextAccessor contextAccessor)
    {
        Context = context;
        _httpContextAccessor = contextAccessor;
    }

    public PayrollDbContext Context { get; }

    public (string, long) GetRoleAndUserId()
    {
        return ((string, long))_httpContextAccessor.HttpContext?.GetRoleAndId()!;
    }

    public T GetRepository<T>() where T : class
    {
        var result = Activator.CreateInstance(typeof(T), Context, _httpContextAccessor);
        return result as T ?? throw new InvalidOperationException("This Error shouldn't Arise!");
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await Context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync(IDbContextTransaction? transaction = null)
    {
        if (transaction != null)
        {
            await transaction.CommitAsync();
        }
        else
        {
            await Context.Database.CommitTransactionAsync();
        }
    }

    public async Task RollBackTransactionAsync(IDbContextTransaction? transaction = null)
    {
        if (transaction != null)
        {
            await transaction.RollbackAsync();
        }
        else
        {
            await Context.Database.RollbackTransactionAsync();
        }

    }

    public async Task SaveAsync()
    {
        await Context.SaveChangesAsync();
    }

    private bool _disposed;

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                Context.Dispose();
            }
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
    }
}
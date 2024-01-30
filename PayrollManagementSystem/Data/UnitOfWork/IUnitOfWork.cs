using Microsoft.EntityFrameworkCore.Storage;
using PayrollManagementSystem.Context;

namespace PayrollManagementSystem.Data.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    Task SaveAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitTransactionAsync(IDbContextTransaction? transaction = null);
    Task RollBackTransactionAsync(IDbContextTransaction? transaction = null);
    T GetRepository<T>() where T : class;
    PayrollDbContext Context { get; }
}
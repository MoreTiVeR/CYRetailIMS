using System.Data;
using CYRetailIMS.Domain.Infrastructure.Repositories;

namespace CYRetailIMS.Domain.Infrastructure.Database;

public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Generic Repository Class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    IGenericRepository<T> Repository<T>() where T : class;

    /// <summary>
    /// Begin Transaction
    /// </summary>
    void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

    /// <summary>
    /// Begin Transaction
    /// </summary>
    Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default);

    /// <summary>
    /// Complete Transaction
    /// </summary>
    void CommitTransaction();

    /// <summary>
    /// Complete Transaction
    /// </summary>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits all changes Return Number of committed.
    /// </summary>
    /// <returns></returns>
    int SaveChanges();

    /// <summary>
    /// Commits all changes Return Number of committed.
    /// </summary>
    Task<int> SaveChangesAsync();

    /// <summary>
    /// Discards all changes that has not been commited
    /// </summary>
    void Rollback();
}

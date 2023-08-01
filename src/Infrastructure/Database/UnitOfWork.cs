using System.ComponentModel.DataAnnotations;
using System.Data;
using CYRetailIMS.Domain.Infrastructure.Database;
using CYRetailIMS.Domain.Infrastructure.Repositories;
using CYRetailIMS.Infrastructure.Common.Extensions;
using CYRetailIMS.Infrastructure.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Infrastructure.Database;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly IMediator _mediator;
    private readonly CYDBContext _dbContext;
    private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();
    public UnitOfWork(CYDBContext context, IMediator mediator) => (_dbContext, _mediator) = (context, mediator);

    public Dictionary<Type, object> Repositories
    {
        get => _repositories;
        set => Repositories = value;
    }
    public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        _dbContext.Database.BeginTransaction(isolationLevel);
    }

    public async Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
    {
        await _dbContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
    }

    public void CommitTransaction()
    {
        _dbContext.Database.CommitTransaction();
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.Database.CommitTransactionAsync(cancellationToken);
    }

    public IGenericRepository<T> Repository<T>() where T : class
    {
        if (Repositories.Keys.Contains(typeof(T)))
        {
            return Repositories[typeof(T)] as IGenericRepository<T>;
        }

        IGenericRepository<T> repo = new GenericRepository<T>(_dbContext);
        Repositories.Add(typeof(T), repo);
        return repo;
    }

    /// <summary>
    /// Rollback/Transaction all changes, modify
    /// </summary>
    public void Rollback()
    {
        foreach (var entry in _dbContext.ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged))
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
                case EntityState.Modified:
                case EntityState.Deleted:
                    entry.Reload();
                    break;
            }
        }
    }

    /// <summary>
    /// Commit Transaction/All changes, modify
    /// </summary>
    /// <returns></returns>
    public int SaveChanges()
    {
        var entities = from e in _dbContext.ChangeTracker.Entries()
                       where e.State == EntityState.Added
                           || e.State == EntityState.Modified
                       select e.Entity;
        foreach (var entity in entities)
        {
            var validationContext = new ValidationContext(entity);
            Validator.ValidateObject(entity, validationContext);
        }

        return _dbContext.SaveChanges();
    }

    /// <summary>
    /// Commit Transaction/All changes, modify
    /// </summary>
    /// <returns></returns>
    public async Task<int> SaveChangesAsync()
    {
        try
        {
            await _mediator.DispatchDomainEvents(_dbContext);
            return await _dbContext.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            Rollback();
            throw;
        }
    }

    public void Dispose() => _dbContext.Dispose();

}

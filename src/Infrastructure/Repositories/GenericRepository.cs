using System.Linq.Expressions;
using CYRetailIMS.Domain.Infrastructure.Repositories;
using CYRetailIMS.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace CYRetailIMS.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly CYDBContext _context;
    public GenericRepository(CYDBContext context) => (_context) = (context);

    public T Add(T entity)
    {
        _context.Set<T>().Add(entity);
        return entity;
    }

    public async Task<T> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        return entity;
    }

    public ICollection<T> AddRange(ICollection<T> entity)
    {
        _context.Set<ICollection<T>>().AddRange(entity);
        return entity;
    }

    public async Task<ICollection<T>> AddRangeAsync(ICollection<T> entity)
    {
        await _context.Set<T>().AddRangeAsync(entity);
        return entity;
    }

    public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entity)
    {
        await _context.Set<T>().AddRangeAsync(entity);
        return entity;
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate) => await _context.Set<T>().AnyAsync(predicate);

    public int Count() => _context.Set<T>().Count();
    public async Task<int> CountAsync() => await _context.Set<T>().CountAsync();

    public void Delete(T t) => _context.Set<T>().Remove(t);

    public void DeleteRange(ICollection<T> t) => _context.Set<T>().RemoveRange(t);

    public int ExecuteSqlRaw(string spName, object[] sqlParameter) => _context.Database.ExecuteSqlRaw(spName, sqlParameter);

    public async Task<int> ExecuteSqlRawAsync(string spName, object[] sqlParameter) => await _context.Database.ExecuteSqlRawAsync(spName, sqlParameter);

    public object[] ExecuteSqlRawWithReturn(string spName, object[] sqlParameter)
    {
        _context.Database.ExecuteSqlRawAsync(spName, sqlParameter);
        return sqlParameter;
    }

    public async Task<object[]> ExecuteSqlRawWithReturnAsync(string spName, object[] sqlParameter)
    {
        await _context.Database.ExecuteSqlRawAsync(spName, sqlParameter);
        return sqlParameter;
    }

    public bool Exist(Expression<Func<T, bool>> predicate) => _context.Set<T>().Where(predicate).Any() ? true : false;

    public IEnumerable<T> Filter(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includeProperties = "",
        int? page = null,
        int? pageSize = null)
    {
        IQueryable<T> query = _context.Set<T>();
        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (
                var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        if (page != null && pageSize != null)
        {
            query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
        }

        return query.AsEnumerable();
    }

    public T Find(Expression<Func<T, bool>> predicate) => _context.Set<T>().FirstOrDefault(predicate);

    public async Task<T> FindAsync(Expression<Func<T, bool>> predicate) => await _context.Set<T>().FirstOrDefaultAsync(predicate);

    public async Task<IEnumerable<T>> FindListAsync(Expression<Func<T, bool>> predicate) => await _context.Set<T>().Where(predicate).ToListAsync();

    public async Task<IEnumerable<T>> FindListAsync(Expression<Func<T, bool>> predicate, string navigationPropertyPath) => await _context.Set<T>().Where(predicate).Include(navigationPropertyPath).ToListAsync();

    public async Task<IQueryable<T>> FindWithInclude(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
        var result = _context.Set<T>().Where(predicate).AsQueryable();

        if (include != null)
            result = include(result);

        return await Task.Run(() => result);
    }

    public async Task<IQueryable<T>> FindWithInclude(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include2 = null)
    {
        var result = _context.Set<T>().Where(predicate).AsQueryable();

        if (include != null)
            result = include(result);

        if (include2 != null)
            result = include2(result);

        return await Task.Run(() => result);
    }

    public async Task<IQueryable<T>> FindWithInclude(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include2 = null
        , Func<IQueryable<T>, IIncludableQueryable<T, object>> include3 = null)
    {
        var result = _context.Set<T>().Where(predicate).AsQueryable();

        if (include != null)
            result = include(result);

        if (include2 != null)
            result = include2(result);

        if (include3 != null)
            result = include3(result);

        return await Task.Run(() => result);
    }

    public async Task<IQueryable<T>> FindWithInclude(Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include2 = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include3 = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include4 = null)
    {
        var result = _context.Set<T>().Where(predicate).AsQueryable();

        if (include != null)
            result = include(result);

        if (include2 != null)
            result = include2(result);

        if (include3 != null)
            result = include3(result);

        if (include4 != null)
            result = include4(result);

        return await Task.Run(() => result);
    }

    public async Task<IQueryable<T>> FindWithInclude(Expression<Func<T, bool>> predicate,
    Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
    Func<IQueryable<T>, IIncludableQueryable<T, object>> include2 = null,
    Func<IQueryable<T>, IIncludableQueryable<T, object>> include3 = null,
    Func<IQueryable<T>, IIncludableQueryable<T, object>> include4 = null,
    Func<IQueryable<T>, IIncludableQueryable<T, object>> include5 = null)
    {
        var result = _context.Set<T>().Where(predicate).AsQueryable();

        if (include != null)
            result = include(result);

        if (include2 != null)
            result = include2(result);

        if (include3 != null)
            result = include3(result);

        if (include4 != null)
            result = include4(result);

        if (include5 != null)
            result = include5(result);

        return await Task.Run(() => result);
    }

    public async Task<IQueryable<T>> FindWithInclude(Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include2 = null, 
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include3 = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include4 = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include5 = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include6 = null)
    {
        var result = _context.Set<T>().Where(predicate).AsQueryable();

        if (include != null)
            result = include(result);

        if (include2 != null)
            result = include2(result);

        if (include3 != null)
            result = include3(result);

        if (include4 != null)
            result = include4(result);

        if (include5 != null)
            result = include5(result);

        if (include6 != null)
            result = include6(result);

        return await Task.Run(() => result);
    }

    public T FirstOrDefault(Expression<Func<T, bool>> predicate) => _context.Set<T>().FirstOrDefault(predicate);

    public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate) => await _context.Set<T>().FirstOrDefaultAsync(predicate);

    public async Task<T> FirstOrDefaultWithIncludeAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
        var result = _context.Set<T>().Where(predicate).AsQueryable();

        if (include != null)
            result = include(result);

        return await Task.Run(() => result.FirstOrDefaultAsync());
    }

    public IEnumerable<T> FromSqlRaw(string spName) => _context.Set<T>().FromSqlRaw(spName).AsEnumerable();

    public IEnumerable<T> FromSqlRaw(string spName, params object[] sqlParameter) => _context.Set<T>().FromSqlRaw(spName, sqlParameter).ToList();

    public async Task<IEnumerable<T>> FromSqlRawAsync(string spName) => await _context.Set<T>().FromSqlRaw(spName).ToListAsync();

    public async Task<IEnumerable<T>> FromSqlRawAsync(string spName, params object[] sqlParameter) => await _context.Set<T>().FromSqlRaw(spName, sqlParameter).ToListAsync();

    public IEnumerable<T> GetAll() => _context.Set<T>().ToList();

    public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

    public async Task<IQueryable<T>> QueryAsync()
    {
        IQueryable<T> query = _context.Set<T>();
        return await Task.Run(() => query);
    }

    public async Task<IQueryable<T>> QueryAsync(Expression<Func<T, bool>> predicate)
    {
        IQueryable<T> query = _context.Set<T>().Where(predicate);
        return await Task.Run(() => query);
    }

    public IQueryable<T> Query()
    {
        IQueryable<T> query = _context.Set<T>();
        return query;
    }

    public IQueryable<T> Query(Expression<Func<T, bool>> predicate) => _context.Set<T>().Where(predicate);

    public T Update(T updated)
    {
        if (updated == null)
        {
            return null;
        }
        _context.Set<T>().Update(updated);
        _context.Entry(updated).State = EntityState.Modified;
        return updated;
    }

    public T UpdateRange(T updated)
    {
        if (updated == null)
        {
            return null;
        }
        _context.Set<T>().UpdateRange(updated);
        _context.Entry(updated).State = EntityState.Modified;
        return updated;
    }

    public IQueryable<T> Where(Expression<Func<T, bool>> predicate) => _context.Set<T>().Where(predicate);
}

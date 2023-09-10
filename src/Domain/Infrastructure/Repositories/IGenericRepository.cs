using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace CYRetailIMS.Domain.Infrastructure.Repositories;

public interface IGenericRepository<T> where T : class
{
	IQueryable<T> Query();
	Task<IQueryable<T>> QueryAsync();

	IQueryable<T> Query(Expression<Func<T, bool>> predicate);
	Task<IQueryable<T>> QueryAsync(Expression<Func<T, bool>> predicate);

	IEnumerable<T> GetAll();

	Task<IEnumerable<T>> GetAllAsync();

	T Find(Expression<Func<T, bool>> match);

	Task<T> FindAsync(Expression<Func<T, bool>> match);

	Task<IQueryable<T>> FindWithInclude(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

	Task<IQueryable<T>> FindWithInclude(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include2 = null);

	Task<IQueryable<T>> FindWithInclude(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include2 = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include3 = null);
    
	Task<IQueryable<T>> FindWithInclude(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include2 = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include3 = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include4 = null);

    T Add(T entity);

	Task<T> AddAsync(T entity);

	ICollection<T> AddRange(ICollection<T> entity);

	Task<ICollection<T>> AddRangeAsync(ICollection<T> entity);

	T Update(T updated);

	T UpdateRange(T updated);

	void Delete(T t);

	void DeleteRange(ICollection<T> t);

	int Count();

	Task<int> CountAsync();

	IEnumerable<T> Filter(
		Expression<Func<T, bool>> filter = null,
		Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
		string includeProperties = "",
		int? page = null,
		int? pageSize = null);

	bool Exist(Expression<Func<T, bool>> predicate);

	T FirstOrDefault(Expression<Func<T, bool>> predicate);
	Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
	Task<IEnumerable<T>> FindListAsync(Expression<Func<T, bool>> predicate);
	Task<IEnumerable<T>> FindListAsync(Expression<Func<T, bool>> predicate, string navigationPropertyPath);
	IQueryable<T> Where(Expression<Func<T, bool>> predicate);
	Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
	IEnumerable<T> FromSqlRaw(string spName);
	IEnumerable<T> FromSqlRaw(string spName, params object[] sqlParameter);
	Task<IEnumerable<T>> FromSqlRawAsync(string spName);
	Task<IEnumerable<T>> FromSqlRawAsync(string spName, params object[] sqlParameter);
	int ExecuteSqlRaw(string spName, object[] sqlParameter);
	Task<int> ExecuteSqlRawAsync(string spName, object[] sqlParameter);

	object[] ExecuteSqlRawWithReturn(string spName, object[] sqlParameter);
	Task<object[]> ExecuteSqlRawWithReturnAsync(string spName, object[] sqlParameter);

}

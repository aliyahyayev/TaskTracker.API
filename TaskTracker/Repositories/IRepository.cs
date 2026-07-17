using System.Linq.Expressions;

namespace TaskTracker.API.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        Task<T?> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveAsync();

        Task<IEnumerable<T>> GetAllPagedAsync(
            System.Linq.Expressions.Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null,
            Func<IQueryable<T>, IQueryable<T>>? orderBy = null,
            int pageNumber = 1,
            int pageSize = 10
        );
    }
}
using System.Linq.Expressions;

namespace DemoImportExport.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        T Add(T t);
        Task<T> AddAsync(T t);
        bool Any();
        Task<bool> AnyAsync();
        int Count();
        Task<int> CountAsync();
        void Delete(T entity);
        Task DeleteAsync(T entity);
#pragma warning disable S2953 // Methods named "Dispose" should implement "IDisposable.Dispose"
        void Dispose();
#pragma warning restore S2953 // Methods named "Dispose" should implement "IDisposable.Dispose"
        T Find(Expression<Func<T, bool>> match);
        T FirstOrDefault(Expression<Func<T, bool>> match);
        ICollection<T> FindAll(Expression<Func<T, bool>> match);
        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match);
        Task<T> FindAsync(Expression<Func<T, bool>> match);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> match);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        Task<ICollection<T>> FindByAsyn(Expression<Func<T, bool>> predicate);
        T Get(object id);
        IQueryable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsyn();
        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetAsync(params object[] id);
        T Update(T t, object key);
        Task<T> UpdateAsync(T t, object key);
    }
}

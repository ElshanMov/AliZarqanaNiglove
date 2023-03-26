using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<int> CommitAsync();
        Task AddAsync(T entity);
        Task<T> GetAsync(Expression<Func<T, bool>> expression, bool tracking = true);
        Task<T> GetAsync(Expression<Func<T, bool>> expression, bool tracking = true, params string[] includes);
        Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> expression, bool tracking = true);
        Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> expression, bool tracking = true,params string[] includes);

        Task Remove(Expression<Func<T, bool>> expression);
    }
}

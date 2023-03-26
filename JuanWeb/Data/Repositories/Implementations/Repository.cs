using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Data.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DataContext _context;
        public DbSet<T> Table => _context.Set<T>();

        public Repository(DataContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public IQueryable<T> GetAll()
        {
            IQueryable<T> query = Table;
            return query;
        }
        public async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> expression, bool tracking = true)
        {
            var query = _context.Set<T>().AsQueryable();
            return await query.Where(expression).ToListAsync();
        }

        public async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> expression, bool tracking = true,params string[] includes)
        {
            var query = Table.AsQueryable();
            for (int i = 0; i < includes.Length; i++)
            {
               query=query.Include(includes[i]);
            }
            return await query.ToListAsync();
        }


        public async Task<T> GetAsync(Expression<Func<T, bool>> expression, bool tracking = true)
        {
            var response = await _context.Set<T>().FirstOrDefaultAsync(expression);
            return response;
        }
        public async Task<T> GetAsync(Expression<Func<T, bool>> expression, bool tracking = true, params string[] includes)
        {
            var query = _context.Set<T>().AsQueryable();
            foreach (var item in includes)
            {
                query = query.Include(item);
            }
            var response = await query.FirstOrDefaultAsync(expression);
            return response;
        }
        public async Task Remove(Expression<Func<T, bool>> expression)
        {
            var entity = await GetAsync(expression);
            _context.Set<T>().Remove(entity);
        }
    }
}

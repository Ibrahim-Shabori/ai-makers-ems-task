using Employee_Management_System.Infrastructure.Data;
using Employee_Management_System.Infrastructure.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Employee_Management_System.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        public DbSet<T> dbSet { get; set; }
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public async Task Add(T entity)
        {
            await dbSet.AddAsync(entity);

        }
        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, string? IncludeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(IncludeProperties))
            {
                foreach (var includeProperty in IncludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<IQueryable<T>> GetAllAsQueryable(Expression<Func<T, bool>>? filter = null, string? IncludeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(IncludeProperties))
            {
                foreach (var includeProperty in IncludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            return await Task.FromResult(query);
        }
        public async Task<T> Get(Expression<Func<T, bool>> filter, string? IncludeProperties = null, bool tracked = false)
        {
            IQueryable<T> query;
            if (tracked == true)
            {
                query = dbSet;
            }
            else
            {
                query = dbSet.AsNoTracking();
            }

            query = query.Where(filter);
            if (!string.IsNullOrEmpty(IncludeProperties))
            {
                foreach (var includeProperty in IncludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> filter)
        {
            return await dbSet.CountAsync(filter);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}

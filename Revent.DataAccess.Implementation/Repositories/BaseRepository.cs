using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Revent.DataAccess.Implementation.DbContexts;
using Revent.DataAccess.Implementation.IRepositories;


namespace Revent.DataAccess.Implementation.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected ReventDbContext _applicationDbContext;
        internal DbSet<T> dbSet { get; set; }


        public BaseRepository(ReventDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            dbSet = _applicationDbContext.Set<T>();
        }
        public virtual async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public virtual async Task DeleteAsync(dynamic id)
        {
            var entity = await dbSet.FindAsync(id);
            dbSet.Remove(entity);
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public virtual async Task<T> GetAsync(dynamic id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual async Task<int> GetCountAsync()
        {
            return await dbSet.CountAsync();
        }

        public virtual async Task<List<T>> GetPagedAsync(int pageNumber, int pageSize)
        {
           return await dbSet
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();
        }

        public virtual void UpdateAsync(T entity)
        {
            dbSet.Update(entity);
        }
    }
}

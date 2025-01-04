using Microsoft.EntityFrameworkCore.ChangeTracking;
using Revent.DataAccess.Implementation.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revent.DataAccess.Implementation.IRepositories
{
        public interface IBaseRepository<T> where T : class
        {
            
            Task AddAsync(T entity);
            void UpdateAsync(T entity);
            Task DeleteAsync(dynamic id);
            Task<T> GetAsync(dynamic id);
            Task<List<T>> GetAllAsync();
            Task<List<T>> GetPagedAsync(int pageNumber, int pageSize);
            Task<int> GetCountAsync();
        }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolManagement.Application.Common.Models;

namespace SchoolManagement.Application.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> ListAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<PaginatedResult<T>> GetPagedAsync(BaseQueryParams queryParams);
    }
}

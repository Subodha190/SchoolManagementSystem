using Microsoft.EntityFrameworkCore;
using SchoolManagement.Application.Repositories;
using SchoolManagement.Application.Common.Interfaces;
using SchoolManagement.Application.Common.Models;

namespace SchoolManagement.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly IApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(IApplicationDbContext context)
        {
            _context = context;
            _dbSet = ((DbContext)context).Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task<IEnumerable<T>> ListAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PaginatedResult<T>> GetPagedAsync(BaseQueryParams queryParams)
        {
            IQueryable<T> query = _dbSet.AsQueryable();

            // Basic filtering by SearchTerm if entity has string properties
            if (!string.IsNullOrWhiteSpace(queryParams.SearchTerm))
            {
                var search = queryParams.SearchTerm.ToLower();
                // Try to build a simple filter across common string properties using LINQ to Objects fallback
                // Note: For simplicity and safety, do a client-side filter when EF cannot translate.
                var list = await query.AsNoTracking().ToListAsync();
                var filtered = list.Where(item =>
                {
                    var props = item.GetType().GetProperties().Where(p => p.PropertyType == typeof(string));
                    foreach (var p in props)
                    {
                        var val = p.GetValue(item) as string;
                        if (!string.IsNullOrWhiteSpace(val) && val.ToLower().Contains(search)) return true;
                    }
                    return false;
                });

                var total = filtered.Count();
                var pageItems = filtered
                    .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                    .Take(queryParams.PageSize)
                    .ToList();

                return new PaginatedResult<T>(pageItems, total, queryParams.PageNumber, queryParams.PageSize);
            }

            // Sorting: attempt to sort by property name if provided
            if (!string.IsNullOrWhiteSpace(queryParams.SortBy))
            {
                var prop = typeof(T).GetProperty(queryParams.SortBy, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (prop != null)
                {
                    query = queryParams.SortDescending
                        ? query.OrderByDescending(x => EF.Property<object>(x, prop.Name))
                        : query.OrderBy(x => EF.Property<object>(x, prop.Name));
                }
            }

            var count = await query.CountAsync();
            var items = await query.Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .AsNoTracking()
                .ToListAsync();

            return new PaginatedResult<T>(items, count, queryParams.PageNumber, queryParams.PageSize);
        }

        // Update
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        // Delete
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
}

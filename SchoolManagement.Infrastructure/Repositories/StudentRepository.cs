    using Microsoft.EntityFrameworkCore;
using SchoolManagement.Application.Common.Interfaces;
using SchoolManagement.Application.Common.Models;
using SchoolManagement.Application.Repositories;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Infrastructure.Repositories
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(IApplicationDbContext context) : base(context) { }

        public async Task<Student?> GetByEmailAsync(string email)
        {
            return await _dbSet.OfType<Student>().FirstOrDefaultAsync(s => s.Email == email);
        }
        public async Task<PaginatedResult<Student>> GetStudentsAsync(BaseQueryParams queryParams)
        {
            IQueryable<Student> query =
                _context.Students
                .Include(x => x.Course)
                .Where(x => !x.IsDeleted);

            // Searching
            if (!string.IsNullOrWhiteSpace(queryParams.SearchTerm))
            {
                var search = queryParams.SearchTerm.ToLower();

                query = query.Where(x =>
                    x.FirstName.ToLower().Contains(search) ||
                    x.LastName.ToLower().Contains(search) ||
                    x.Email.ToLower().Contains(search) ||
                    x.PhoneNumber.Contains(search));
            }

            // Sorting
            query = queryParams.SortBy?.ToLower() switch
            {
                "firstname" => queryParams.SortDescending
                    ? query.OrderByDescending(x => x.FirstName)
                    : query.OrderBy(x => x.FirstName),

                "lastname" => queryParams.SortDescending
                    ? query.OrderByDescending(x => x.LastName)
                    : query.OrderBy(x => x.LastName),

                "email" => queryParams.SortDescending
                    ? query.OrderByDescending(x => x.Email)
                    : query.OrderBy(x => x.Email),

                "createdat" => queryParams.SortDescending
                    ? query.OrderByDescending(x => x.CreatedAt)
                    : query.OrderBy(x => x.CreatedAt),

                _ => query.OrderBy(x => x.Id)
            };

            var totalCount = await query.CountAsync();

            var students = await query
                .Skip((queryParams.PageNumber - 1)
                    * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToListAsync();

            return new PaginatedResult<Student>(
                students,
                totalCount,
                queryParams.PageNumber,
                queryParams.PageSize);
        }
    }
}

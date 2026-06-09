using Microsoft.EntityFrameworkCore;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Student> Students { get; }

        DbSet<Course> Courses { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

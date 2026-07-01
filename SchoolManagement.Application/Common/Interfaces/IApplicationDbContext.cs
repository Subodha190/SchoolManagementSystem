using Microsoft.EntityFrameworkCore;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Student> Students { get; }

        DbSet<Course> Courses { get; }
        DbSet<RefreshToken> RefreshTokens { get; }

        DbSet<Teacher> Teachers { get; }

        DbSet<School> Schools { get; }

        DbSet<Enrollment> Enrollments { get; }

        DbSet<Subject> Subjects { get; }

        DbSet<Attendance> Attendances { get; }

        DbSet<FeePayment> FeePayments { get; }

        DbSet<Permission> Permissions { get; }

        DbSet<RolePermission> RolePermissions { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Application.Repositories
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        IStudentRepository Students { get; }
        IRepository<ApplicationUser> Users { get; }
        IRepository<Course> Courses { get; }
        IRepository<Teacher> Teachers { get; }
        IRepository<School> Schools { get; }
        IRepository<Subject> Subjects { get; }
        IRepository<Enrollment> Enrollments { get; }
        IRepository<Attendance> Attendances { get; }
        IRepository<FeePayment> FeePayments { get; }
        IRepository<ApplicationUser> ApplicationUsers { get; }

    }
}

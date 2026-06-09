using SchoolManagement.Application.Common.Interfaces;
using SchoolManagement.Application.Repositories;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Infrastructure.Persistence;

namespace SchoolManagement.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IStudentRepository Students { get; }
        public IRepository<Course> Courses { get; }
        public IRepository<Teacher> Teachers { get; }
        public IRepository<School> Schools { get; }
        public IRepository<Subject> Subjects { get; }
        public IRepository<Enrollment> Enrollments { get; }
        public IRepository<Attendance> Attendances { get; }
        public IRepository<FeePayment> FeePayments { get; }

        public UnitOfWork(
            ApplicationDbContext context,
            IStudentRepository studentRepository,
            IRepository<Course> courseRepository,
            IRepository<Teacher> teacherRepository,
            IRepository<School> schoolRepository,
            IRepository<Subject> subjectRepository,
            IRepository<Enrollment> enrollmentRepository,
            IRepository<Attendance> attendanceRepository,
            IRepository<FeePayment> feePaymentRepository)
        {
            _context = context;
            Students = studentRepository;
            Courses = courseRepository;
            Teachers = teacherRepository;
            Schools = schoolRepository;
            Subjects = subjectRepository;
            Enrollments = enrollmentRepository;
            Attendances = attendanceRepository;
            FeePayments = feePaymentRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}

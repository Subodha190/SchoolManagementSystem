using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchoolManagement.Application.Common.Interfaces;
using SchoolManagement.Application.Repositories;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Infrastructure.Persistence;
using SchoolManagement.Infrastructure.Repositories;

namespace SchoolManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IRepository<>),
                typeof(Repository<>));

            // Expose ApplicationDbContext through the IApplicationDbContext interface
            services.AddScoped<IApplicationDbContext>(provider =>
                provider.GetRequiredService<ApplicationDbContext>());

            services.AddScoped<IStudentRepository,
                StudentRepository>();

            // Student service registration moved here to centralize infrastructure registrations
            services.AddScoped<SchoolManagement.Application.Common.Interfaces.IStudentService, SchoolManagement.Application.Services.StudentService>();
            
            

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register repositories for other entities where needed
            services.AddScoped<IRepository<Teacher>, Repository<Teacher>>();
            services.AddScoped<IRepository<School>, Repository<School>>();
            services.AddScoped<IRepository<Subject>, Repository<Subject>>();
            services.AddScoped<IRepository<Enrollment>, Repository<Enrollment>>();
            services.AddScoped<IRepository<Attendance>, Repository<Attendance>>();
            services.AddScoped<IRepository<FeePayment>, Repository<FeePayment>>();

            // Generic CRUD services for simple repositories
            services.AddScoped(typeof(SchoolManagement.Application.Services.GenericCrudService<>));

            // Register entity services (implementation classes)
            services.AddScoped<SchoolManagement.Application.Common.Interfaces.ICourseService, SchoolManagement.Application.Services.CourseServiceImpl>();
            services.AddScoped<SchoolManagement.Application.Common.Interfaces.ITeacherService, SchoolManagement.Application.Services.TeacherServiceImpl>();
            services.AddScoped<SchoolManagement.Application.Common.Interfaces.ISchoolService, SchoolManagement.Application.Services.SchoolServiceImpl>();
            services.AddScoped<SchoolManagement.Application.Common.Interfaces.ISubjectService, SchoolManagement.Application.Services.SubjectServiceImpl>();
            services.AddScoped<SchoolManagement.Application.Common.Interfaces.IEnrollmentService, SchoolManagement.Application.Services.EnrollmentServiceImpl>();
            services.AddScoped<SchoolManagement.Application.Common.Interfaces.IAttendanceService, SchoolManagement.Application.Services.AttendanceServiceImpl>();
            services.AddScoped<SchoolManagement.Application.Common.Interfaces.IFeePaymentService, SchoolManagement.Application.Services.FeePaymentServiceImpl>();

            return services;
        }
    }
}

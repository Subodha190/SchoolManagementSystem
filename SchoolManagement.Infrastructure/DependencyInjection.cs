using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchoolManagement.Application.Common.Interfaces;
using SchoolManagement.Application.Repositories;
using SchoolManagement.Application.Services;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Infrastructure.MultiTenancy;
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

            // Identity and auth services
            services.AddScoped<SchoolManagement.Application.Common.Interfaces.IJwtTokenService, SchoolManagement.Application.Services.JwtTokenService>();
            services.AddScoped<SchoolManagement.Application.Common.Interfaces.IRefreshTokenService, SchoolManagement.Infrastructure.Services.RefreshTokenService>();
            // Auth service
            services.AddScoped<SchoolManagement.Application.Services.AuthService>();
           

            // Register repositories for other entities where needed
            services.AddScoped<IRepository<Teacher>, Repository<Teacher>>();
            services.AddScoped<IRepository<School>, Repository<School>>();
            services.AddScoped<IRepository<Subject>, Repository<Subject>>();
            services.AddScoped<IRepository<Enrollment>, Repository<Enrollment>>();
            services.AddScoped<IRepository<Attendance>, Repository<Attendance>>();
            services.AddScoped<IRepository<FeePayment>, Repository<FeePayment>>();
            services.AddScoped<IJwtTokenService,JwtTokenService>();

            // Generic CRUD services for simple repositories
            services.AddScoped(typeof(SchoolManagement.Application.Services.GenericCrudService<>));

            // Register entity services (implementation classes) - implementations below
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ITeacherService,TeacherService>();
            services.AddScoped<ISchoolService, SchoolService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<IEnrollmentService, EnrollmentService>();
            services.AddScoped<IAttendanceService, AttendanceService>();
            services.AddScoped<IFeePaymentService, FeePaymentService>();
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentTenantService, CurrentTenantService>();

            // Register dynamic Permission policy provider and authorization handler
            // PermissionAuthorizationHandler now resolves scoped services via IServiceScopeFactory,
            // therefore it is safe to register it as a singleton.
            services.AddSingleton<Microsoft.AspNetCore.Authorization.IAuthorizationPolicyProvider, SchoolManagement.Infrastructure.Authorization.PermissionPolicyProvider>();
            services.AddSingleton<Microsoft.AspNetCore.Authorization.IAuthorizationHandler, SchoolManagement.Infrastructure.Authorization.PermissionAuthorizationHandler>();

            services.AddMemoryCache();

            // Request context for audit and db context use
            services.AddScoped<SchoolManagement.Application.Common.Interfaces.IRequestContext, SchoolManagement.Infrastructure.Request.RequestContext>();
            services.AddScoped<SchoolManagement.Application.Common.Interfaces.IPermissionService, SchoolManagement.Infrastructure.Services.PermissionService>();

            // Note: Do NOT set CurrentSchoolId here in DI time. Tenant is resolved per request
            // by TenantResolutionMiddleware which will set the ApplicationDbContext.CurrentSchoolId.

           


            return services;
        }
    }
}

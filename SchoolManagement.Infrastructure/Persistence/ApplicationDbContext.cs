using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SchoolManagement.Application.Common.Interfaces;
using SchoolManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SchoolManagement.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>, IApplicationDbContext
    {
        // Current tenant (school) id used by global query filters. 0 = global/system
        public int CurrentSchoolId { get; private set; } = 0;

        public void SetCurrentSchoolId(int schoolId)
        {
            CurrentSchoolId = schoolId;
        }

        private readonly SchoolManagement.Application.Common.Interfaces.IRequestContext _requestContext;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, SchoolManagement.Application.Common.Interfaces.IRequestContext requestContext) : base(options)
        {
            _requestContext = requestContext;
        }

        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Teacher> Teachers { get; set; } = null!;
        public DbSet<School> Schools { get; set; } = null!;
        public DbSet<Enrollment> Enrollments { get; set; } = null!;
        public DbSet<Subject> Subjects { get; set; } = null!;
        public DbSet<Attendance> Attendances { get; set; } = null!;
        public DbSet<FeePayment> FeePayments { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<SchoolManagement.Infrastructure.Auditing.AuditLog> AuditLogs { get; set; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            // Populate audit fields and SchoolId for new entities
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.Entity is Domain.Common.AuditableEntity auditable)
                {
                    var now = DateTime.UtcNow;
                    if (entry.State == EntityState.Added)
                    {
                        auditable.CreatedAt = now;
                        auditable.CreatedBy = auditable.CreatedBy ?? "system";
                        // set tenant id on creation (0 means global)
                        if (auditable.SchoolId == 0)
                            auditable.SchoolId = CurrentSchoolId;
                    }

                    if (entry.State == EntityState.Modified)
                    {
                        auditable.UpdatedAt = now;
                        auditable.UpdatedBy = auditable.UpdatedBy ?? "system";
                    }
                }

                // Special-case RefreshToken which may not inherit AuditableEntity
                if (entry.Entity is Domain.Entities.RefreshToken rt)
                {
                    if (entry.State == EntityState.Added && rt.SchoolId == 0)
                        rt.SchoolId = CurrentSchoolId;
                }
                // Create audit log entries for Added/Modified/Deleted
                if (entry.Entity != null && !(entry.Entity is SchoolManagement.Infrastructure.Auditing.AuditLog) && (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted))
                {
                    try
                    {
                        var audit = new SchoolManagement.Infrastructure.Auditing.AuditLog
                        {
                            SchoolId = CurrentSchoolId,
                            UserId = _requestContext?.UserId,
                            UserName = _requestContext?.UserName ?? string.Empty,
                            Action = entry.State.ToString(),
                            EntityName = entry.Entity.GetType().Name,
                            EntityId = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey())?.CurrentValue?.ToString() ?? string.Empty,
                            OldValues = entry.State == EntityState.Added ? string.Empty : System.Text.Json.JsonSerializer.Serialize(entry.OriginalValues.Properties.ToDictionary(p => p.Name, p => entry.OriginalValues[p.Name])),
                            NewValues = entry.State == EntityState.Deleted ? string.Empty : System.Text.Json.JsonSerializer.Serialize(entry.CurrentValues.Properties.ToDictionary(p => p.Name, p => entry.CurrentValues[p.Name])),
                            IPAddress = _requestContext?.IPAddress ?? string.Empty,
                            UserAgent = _requestContext?.UserAgent ?? string.Empty,
                            TraceId = _requestContext?.TraceId ?? string.Empty,
                            CorrelationId = _requestContext?.CorrelationId ?? string.Empty,
                            CreatedAt = DateTime.UtcNow
                        };

                        // Add audit record to context; will be saved with the rest of changes
                        this.Set<SchoolManagement.Infrastructure.Auditing.AuditLog>().Add(audit);
                    }
                    catch
                    {
                        // ensure audit errors do not block main transaction; swallow safely
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ApplicationDbContext).Assembly);

            // Ensure no cascade delete paths cause issues by setting all FKs to Restrict
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            // Additional model configurations can go here

            // Global query filters for soft-delete and tenant isolation (SchoolId)
            modelBuilder.Entity<Student>()
                .HasQueryFilter(x => !x.IsDeleted && (x.SchoolId == 0 || x.SchoolId == CurrentSchoolId));

            modelBuilder.Entity<Course>()
                .HasQueryFilter(x => !x.IsDeleted && (x.SchoolId == 0 || x.SchoolId == CurrentSchoolId));

            modelBuilder.Entity<Teacher>()
                .HasQueryFilter(x => !x.IsDeleted && (x.SchoolId == 0 || x.SchoolId == CurrentSchoolId));

            modelBuilder.Entity<School>()
                .HasQueryFilter(x => !x.IsDeleted && (x.SchoolId == 0 || x.SchoolId == CurrentSchoolId));

            modelBuilder.Entity<Enrollment>()
                .HasQueryFilter(x => !x.IsDeleted && (x.SchoolId == 0 || x.SchoolId == CurrentSchoolId));

            modelBuilder.Entity<Subject>()
                .HasQueryFilter(x => !x.IsDeleted && (x.SchoolId == 0 || x.SchoolId == CurrentSchoolId));

            modelBuilder.Entity<Attendance>()
                .HasQueryFilter(x => !x.IsDeleted && (x.SchoolId == 0 || x.SchoolId == CurrentSchoolId));

            modelBuilder.Entity<FeePayment>()
                .HasQueryFilter(x => !x.IsDeleted && (x.SchoolId == 0 || x.SchoolId == CurrentSchoolId));

            modelBuilder.Entity<RefreshToken>()
                .HasQueryFilter(x => x.SchoolId == 0 || x.SchoolId == CurrentSchoolId);

            // Configure relationships to avoid multiple cascade delete paths
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Course)
                .WithMany(c => c.Students)
                .HasForeignKey(s => s.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany()
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany()
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

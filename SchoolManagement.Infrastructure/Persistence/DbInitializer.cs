using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Infrastructure.Identity;

namespace SchoolManagement.Infrastructure.Persistence
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(
            IServiceProvider services)
        {
            using var scope =
                services.CreateScope();

            var roleManager =
                scope.ServiceProvider
                    .GetRequiredService<
                        RoleManager<IdentityRole<int>>>();

            var userManager =
                scope.ServiceProvider
                    .GetRequiredService<
                        UserManager<ApplicationUser>>();

            await RoleSeeder.SeedRolesAsync(
                roleManager);

            var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await UserSeeder.SeedSuperAdminAsync(
                roleManager,
                userManager,
                appContext);

            // Seed permissions and role permissions
            if (!appContext.Permissions.Any())
            {
                var permissions = new List<Permission>
                {
                    new Permission { ModuleName = "Student", ActionName = "Create", PermissionKey = "Student.Create" },
                    new Permission { ModuleName = "Student", ActionName = "Update", PermissionKey = "Student.Update" },
                    new Permission { ModuleName = "Student", ActionName = "Delete", PermissionKey = "Student.Delete" },
                    new Permission { ModuleName = "Teacher", ActionName = "Manage", PermissionKey = "Teacher.Manage" },
                    new Permission { ModuleName = "School", ActionName = "Manage", PermissionKey = "School.Manage" }
                };

                appContext.Permissions.AddRange(permissions);
                await appContext.SaveChangesAsync();

                // Assign all permissions to SuperAdmin role
                var superRole = await appContext.Roles.FirstOrDefaultAsync(r => r.Name == "SuperAdmin");
                if (superRole != null)
                {
                    foreach (var p in permissions)
                    {
                        appContext.RolePermissions.Add(new RolePermission { RoleId = superRole.Id, PermissionId = p.Id });
                    }
                }

                // Assign student permissions to SchoolAdmin
                var schoolRole = await appContext.Roles.FirstOrDefaultAsync(r => r.Name == "SchoolAdmin");
                var studentPerm = permissions.Where(p => p.ModuleName == "Student").ToList();
                if (schoolRole != null)
                {
                    foreach (var p in studentPerm)
                    {
                        appContext.RolePermissions.Add(new RolePermission { RoleId = schoolRole.Id, PermissionId = p.Id });
                    }
                }

                await appContext.SaveChangesAsync();
            }

            // Seed some sample data: a school, a school admin, a teacher and a student
            if (!appContext.Schools.Any())
            {
                var school = new School
                {
                    SchoolCode = "SCH1001",
                    SchoolName = "Central High",
                    Email = "info@centralhigh.com",
                    IsActive = true
                };

                appContext.Schools.Add(school);
                await appContext.SaveChangesAsync();

                // create a school admin user tied to this school
                var adminEmail = "schooladmin@centralhigh.com";
                var existing = await userManager.FindByEmailAsync(adminEmail);
                if (existing == null)
                {
                    var adminUser = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        FullName = "Central High Admin",
                        EmailConfirmed = true,
                        SchoolId = school.Id
                    };

                    var createResult = await userManager.CreateAsync(adminUser, "Admin@123");
                    if (createResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "SchoolAdmin");
                    }
                }

                // add a teacher
                var teacher = new Teacher
                {
                    FullName  = "John Doe",
                    Email = "j.doe@centralhigh.com",
                    Salary = 50000m,
                    SchoolId = school.Id
                };
                appContext.Teachers.Add(teacher);

                // add a student
                var student = new Student
                {
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@student.com",
                    SchoolId = school.Id
                   
                };
                appContext.Students.Add(student);

                await appContext.SaveChangesAsync();
            }
        }
    }
}

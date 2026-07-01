using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SchoolManagement.Application.Common.Interfaces;
using SchoolManagement.Application.Common.Models;
using SchoolManagement.Application.Repositories;
using SchoolManagement.Application.Services;
using SchoolManagement.Infrastructure;
using SchoolManagement.Infrastructure.Identity;
using SchoolManagement.Infrastructure.Persistence;
using SchoolManagement.Infrastructure.Repositories;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

#region Controllers + JSON
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        opts.JsonSerializerOptions.MaxDepth = 64;
    });
#endregion

#region Infrastructure (DbContext, etc.)
builder.Services.AddInfrastructure(builder.Configuration);
#endregion

#region Identity (ONLY ONCE)
builder.Services.AddIdentityServices();
#endregion

#region Jwt Settings
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));
#endregion


// ✅ ONLY THIS LINE (your extension method)
builder.Services.AddJwtAuthentication(builder.Configuration);


#region Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.AddPolicy("RequireSchoolAdmin",
        policy => policy.RequireRole("SchoolAdmin", "SuperAdmin"));

    options.AddPolicy("RequireTeacherOrAdmin",
        policy => policy.RequireRole("Teacher", "SchoolAdmin", "SuperAdmin"));
});
#endregion

#region FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateStudentValidator>();
builder.Services.AddScoped<ValidationFilter<CreateStudentDto>>();
#endregion

#region AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<SchoolManagement.Application.Mapping.MappingProfile>());
#endregion

#region DI (Repositories + Services)
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();

#endregion

#region Swagger + JWT Support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "School Management API",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});
#endregion

var app = builder.Build();

#region Seed Data
using (var scope = app.Services.CreateScope())
{
    await DbInitializer.SeedAsync(scope.ServiceProvider);
}
#endregion

#region Middleware Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "School Management API V1");
    c.RoutePrefix = string.Empty;
});

app.UseRouting();

app.UseMiddleware<ExceptionMiddleware>();
// Tenant resolution must occur after authentication so the SchoolId claim is available
app.UseAuthentication();
app.UseMiddleware<SchoolManagement.API.Middleware.TenantResolutionMiddleware>();

app.UseAuthorization();

app.MapControllers();
#endregion

app.Run();
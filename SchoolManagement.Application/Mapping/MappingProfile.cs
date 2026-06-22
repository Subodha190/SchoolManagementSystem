using AutoMapper;
using SchoolManagement.Application.Common.Models;
using SchoolManagement.Domain.Entities;
using System;

namespace SchoolManagement.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateStudentDto, Student>();

            CreateMap<CreateCourseDto, Course>()
                .ForMember(dest => dest.CourseCode, opt => opt.MapFrom(src => src.CourseCode))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Fees, opt => opt.MapFrom(src => src.Fees))
                .ForMember(dest => dest.DurationInMonths, opt => opt.MapFrom(src => src.DurationInMonths));

            CreateMap<Course, CourseResponseDto>();

            CreateMap<CreateTeacherDto, Teacher>()
                .ForMember(dest => dest.EmployeeCode, opt => opt.MapFrom(src => src.EmployeeCode))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.Salary))
                .ForMember(dest => dest.SubjectSpecialization, opt => opt.MapFrom(src => src.SubjectSpecialization));

            CreateMap<Teacher, TeacherResponseDto>();

            CreateMap<CreateSchoolDto, School>();
            CreateMap<School, SchoolResponseDto>();

            CreateMap<CreateSubjectDto, Subject>();
            CreateMap<Subject, SubjectResponseDto>();

            CreateMap<CreateEnrollmentDto, Enrollment>()
                .ForMember(dest => dest.EnrollmentDate, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<Enrollment, EnrollmentResponseDto>()
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.StudentId))
                .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId));

            CreateMap<CreateAttendanceDto, Attendance>();
            CreateMap<Attendance, AttendanceResponseDto>();

            CreateMap<CreateFeePaymentDto, FeePayment>();
            CreateMap<FeePayment, FeePaymentResponseDto>();

            CreateMap<Student, StudentResponseDto>()
                .ForMember(
                    dest => dest.FullName,
                    opt => opt.MapFrom(
                        src => src.FirstName + " " + src.LastName))
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<ApplicationUser, ApplicationUserDto>();
        }
    }
}

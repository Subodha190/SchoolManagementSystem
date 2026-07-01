using AutoMapper;
using SchoolManagement.Application.Common.Interfaces;
using SchoolManagement.Application.Common.Models;
using SchoolManagement.Application.Repositories;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Enums;

namespace SchoolManagement.Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public StudentService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<StudentResponseDto> CreateAsync(
            CreateStudentDto dto)
        {
            var existingStudent =
                await _unitOfWork.Students
                    .GetByEmailAsync(dto.Email);

            if (existingStudent != null)
            {
                throw new Exception("Student email already exists");
            }

            var student = _mapper.Map<Student>(dto);

            student.CreatedAt = DateTime.UtcNow;
            student.CreatedBy = "system";

            student.Status = StudentStatus.Active;

            await _unitOfWork.Students.AddAsync(student);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<StudentResponseDto>(student);
        }

        public async Task<IEnumerable<StudentResponseDto>> GetAllAsync()
        {
            var students = await _unitOfWork.Students.ListAsync();

            return _mapper.Map<IEnumerable<StudentResponseDto>>(students);
        }

        public async Task<StudentResponseDto> GetByIdAsync(int id)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(id);

            if (student == null)
            {
                throw new Exception("Student not found");
            }

            return _mapper.Map<StudentResponseDto>(student);
        }

        public async Task UpdateAsync(UpdateStudentDto dto)
        {
            var student = await _unitOfWork.Students.GetByIdAsync((int)dto.Id);

            if (student == null)
            {
                throw new Exception("Student not found");
            }
            // Update properties
            student.FirstName = dto.FirstName;
            student.LastName = dto.LastName;
            student.Email = dto.Email;
            student.UpdatedAt = DateTime.UtcNow;
            student.UpdatedBy = "system";

            _unitOfWork.Students.Update(student);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PaginatedResult<StudentResponseDto>> GetStudentsAsync(BaseQueryParams queryParams)
        {
            var paged = await _unitOfWork.Students.GetStudentsAsync(queryParams);
            var mapped = _mapper.Map<IEnumerable<StudentResponseDto>>(paged.Items);
            return new PaginatedResult<StudentResponseDto>(mapped, paged.TotalCount, paged.PageNumber, paged.PageSize);
        }
    }
}

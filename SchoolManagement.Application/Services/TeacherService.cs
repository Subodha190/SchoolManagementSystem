using AutoMapper;
using SchoolManagement.Application.Common.Models;
using SchoolManagement.Application.Repositories;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Application.Services
{
    public class TeacherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TeacherService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TeacherResponseDto> CreateAsync(CreateTeacherDto dto)
        {
            var entity = _mapper.Map<Teacher>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = "system";
            await _unitOfWork.Teachers.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TeacherResponseDto>(entity);
        }

        public async Task<IEnumerable<TeacherResponseDto>> GetAllAsync()
        {
            var list = await _unitOfWork.Teachers.ListAsync();
            return _mapper.Map<IEnumerable<TeacherResponseDto>>(list);
        }

        public async Task<PaginatedResult<TeacherResponseDto>> GetPagedAsync(BaseQueryParams queryParams)
        {
            var paged = await _unitOfWork.Teachers.GetPagedAsync(queryParams);
            var mapped = _mapper.Map<IEnumerable<TeacherResponseDto>>(paged.Items);
            return new PaginatedResult<TeacherResponseDto>(mapped, paged.TotalCount, paged.PageNumber, paged.PageSize);
        }

        public async Task<TeacherResponseDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Teachers.GetByIdAsync(id);
            if (entity == null) throw new Exception("Teacher not found");
            return _mapper.Map<TeacherResponseDto>(entity);
        }

        public async Task UpdateAsync(UpdateTeacherDto dto)
        {
            var entity = await _unitOfWork.Teachers.GetByIdAsync((int)dto.Id);
            if (entity == null) throw new Exception("Teacher not found");
            entity.EmployeeCode = dto.EmployeeCode;
            entity.FullName = dto.FullName;
            entity.Email = dto.Email;
            entity.PhoneNumber = dto.PhoneNumber;
            entity.Salary = dto.Salary;
            entity.SubjectSpecialization = dto.SubjectSpecialization;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = "system";
            _unitOfWork.Teachers.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Teachers.GetByIdAsync(id);
            if (entity == null) throw new Exception("Teacher not found");
            _unitOfWork.Teachers.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

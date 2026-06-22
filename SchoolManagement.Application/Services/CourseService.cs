using AutoMapper;
using SchoolManagement.Application.Common.Interfaces;
using SchoolManagement.Application.Common.Models;
using SchoolManagement.Application.Repositories;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CourseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CourseResponseDto> CreateAsync(CreateCourseDto dto)
        {
            var entity = _mapper.Map<Course>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = "system";
            await _unitOfWork.Courses.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CourseResponseDto>(entity);
        }

        public async Task<IEnumerable<CourseResponseDto>> GetAllAsync()
        {
            var list = await _unitOfWork.Courses.ListAsync();
            return _mapper.Map<IEnumerable<CourseResponseDto>>(list);
        }

        public async Task<PaginatedResult<CourseResponseDto>> GetPagedAsync(BaseQueryParams queryParams)
        {
            var paged = await _unitOfWork.Courses.GetPagedAsync(queryParams);
            var mapped = _mapper.Map<IEnumerable<CourseResponseDto>>(paged.Items);
            return new PaginatedResult<CourseResponseDto>(mapped, paged.TotalCount, paged.PageNumber, paged.PageSize);
        }

        public async Task<CourseResponseDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Courses.GetByIdAsync(id);
            if (entity == null) throw new Exception("Course not found");
            return _mapper.Map<CourseResponseDto>(entity);
        }

        public async Task UpdateAsync(UpdateCourseDto dto)
        {
            var entity = await _unitOfWork.Courses.GetByIdAsync((int)dto.Id);
            if (entity == null) throw new Exception("Course not found");
            entity.CourseCode = dto.CourseCode;
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.Fees = dto.Fees;
            entity.DurationInMonths = dto.DurationInMonths;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = "system";
            _unitOfWork.Courses.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Courses.GetByIdAsync(id);
            if (entity == null) throw new Exception("Course not found");
            _unitOfWork.Courses.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

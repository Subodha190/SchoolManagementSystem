using AutoMapper;
using SchoolManagement.Application.Common.Interfaces;
using SchoolManagement.Application.Common.Models;
using SchoolManagement.Application.Repositories;
using SchoolManagement.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Services
{
    public class EnrollmentServiceImpl : IEnrollmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EnrollmentServiceImpl(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EnrollmentResponseDto> CreateAsync(CreateEnrollmentDto dto)
        {
            var entity = _mapper.Map<Enrollment>(dto);
            entity.EnrollmentDate = System.DateTime.UtcNow;
            entity.CreatedAt = System.DateTime.UtcNow;
            entity.CreatedBy = "system";
            await _unitOfWork.Enrollments.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<EnrollmentResponseDto>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Enrollments.GetByIdAsync(id);
            if (entity == null) throw new System.Exception("Enrollment not found");
            _unitOfWork.Enrollments.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<EnrollmentResponseDto>> GetAllAsync()
        {
            var list = await _unitOfWork.Enrollments.ListAsync();
            return _mapper.Map<IEnumerable<EnrollmentResponseDto>>(list);
        }

        public async Task<EnrollmentResponseDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Enrollments.GetByIdAsync(id);
            if (entity == null) throw new System.Exception("Enrollment not found");
            return _mapper.Map<EnrollmentResponseDto>(entity);
        }

        public async Task UpdateAsync(UpdateEnrollmentDto dto)
        {
            var entity = await _unitOfWork.Enrollments.GetByIdAsync(dto.Id);
            if (entity == null) throw new System.Exception("Enrollment not found");
            entity.StudentId = dto.StudentId;
            entity.CourseId = dto.CourseId;
            entity.UpdatedAt = System.DateTime.UtcNow;
            entity.UpdatedBy = "system";
            _unitOfWork.Enrollments.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PaginatedResult<EnrollmentResponseDto>> GetPagedAsync(BaseQueryParams queryParams)
        {
            var paged = await _unitOfWork.Enrollments.GetPagedAsync(queryParams);
            var mapped = _mapper.Map<IEnumerable<EnrollmentResponseDto>>(paged.Items);
            return new PaginatedResult<EnrollmentResponseDto>(mapped, paged.TotalCount, paged.PageNumber, paged.PageSize);
        }
    }
}

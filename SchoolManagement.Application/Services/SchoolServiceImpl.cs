using AutoMapper;
using SchoolManagement.Application.Common.Interfaces;
using SchoolManagement.Application.Common.Models;
using SchoolManagement.Application.Repositories;
using SchoolManagement.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Services
{
    public class SchoolServiceImpl : ISchoolService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SchoolServiceImpl(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SchoolResponseDto> CreateAsync(CreateSchoolDto dto)
        {
            var entity = _mapper.Map<School>(dto);
            entity.CreatedAt = System.DateTime.UtcNow;
            entity.CreatedBy = "system";
            await _unitOfWork.Schools.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<SchoolResponseDto>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Schools.GetByIdAsync(id);
            if (entity == null) throw new System.Exception("School not found");
            _unitOfWork.Schools.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<SchoolResponseDto>> GetAllAsync()
        {
            var list = await _unitOfWork.Schools.ListAsync();
            return _mapper.Map<IEnumerable<SchoolResponseDto>>(list);
        }

        public async Task<SchoolResponseDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Schools.GetByIdAsync(id);
            if (entity == null) throw new System.Exception("School not found");
            return _mapper.Map<SchoolResponseDto>(entity);
        }

        public async Task UpdateAsync(UpdateSchoolDto dto)
        {
            var entity = await _unitOfWork.Schools.GetByIdAsync(dto.Id);
            if (entity == null) throw new System.Exception("School not found");
            entity.SchoolName = dto.Name;
            entity.UpdatedAt = System.DateTime.UtcNow;
            entity.UpdatedBy = "system";
            _unitOfWork.Schools.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PaginatedResult<SchoolResponseDto>> GetPagedAsync(BaseQueryParams queryParams)
        {
            var paged = await _unitOfWork.Schools.GetPagedAsync(queryParams);
            var mapped = _mapper.Map<IEnumerable<SchoolResponseDto>>(paged.Items);
            return new PaginatedResult<SchoolResponseDto>(mapped, paged.TotalCount, paged.PageNumber, paged.PageSize);
        }
    }
}

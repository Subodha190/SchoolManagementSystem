using AutoMapper;
using SchoolManagement.Application.Common.Interfaces;
using SchoolManagement.Application.Common.Models;
using SchoolManagement.Application.Repositories;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Application.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubjectService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SubjectResponseDto> CreateAsync(CreateSubjectDto dto)
        {
            var entity = _mapper.Map<Subject>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = "system";
            await _unitOfWork.Subjects.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<SubjectResponseDto>(entity);
        }

        public async Task<IEnumerable<SubjectResponseDto>> GetAllAsync()
        {
            var list = await _unitOfWork.Subjects.ListAsync();
            return _mapper.Map<IEnumerable<SubjectResponseDto>>(list);
        }

        public async Task<PaginatedResult<SubjectResponseDto>> GetPagedAsync(BaseQueryParams queryParams)
        {
            var paged = await _unitOfWork.Subjects.GetPagedAsync(queryParams);
            var mapped = _mapper.Map<IEnumerable<SubjectResponseDto>>(paged.Items);
            return new PaginatedResult<SubjectResponseDto>(mapped, paged.TotalCount, paged.PageNumber, paged.PageSize);
        }

        public async Task<SubjectResponseDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Subjects.GetByIdAsync(id);
            if (entity == null) throw new Exception("Subject not found");
            return _mapper.Map<SubjectResponseDto>(entity);
        }

        public async Task UpdateAsync(UpdateSubjectDto dto)
        {
            var entity = await _unitOfWork.Subjects.GetByIdAsync(dto.Id);
            if (entity == null) throw new Exception("Subject not found");
            entity.SubjectCode = dto.SubjectCode;
            entity.Name = dto.Name;
            entity.Credits = dto.Credits;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = "system";
            _unitOfWork.Subjects.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Subjects.GetByIdAsync(id);
            if (entity == null) throw new Exception("Subject not found");
            _unitOfWork.Subjects.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

using AutoMapper;
using SchoolManagement.Application.Common.Interfaces;
using SchoolManagement.Application.Common.Models;
using SchoolManagement.Application.Repositories;
using SchoolManagement.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Services
{
    public class AttendanceServiceImpl : IAttendanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AttendanceServiceImpl(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AttendanceResponseDto> CreateAsync(CreateAttendanceDto dto)
        {
            var entity = _mapper.Map<Attendance>(dto);
            entity.CreatedAt = System.DateTime.UtcNow;
            entity.CreatedBy = "system";
            await _unitOfWork.Attendances.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<AttendanceResponseDto>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Attendances.GetByIdAsync(id);
            if (entity == null) throw new System.Exception("Attendance not found");
            _unitOfWork.Attendances.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<AttendanceResponseDto>> GetAllAsync()
        {
            var list = await _unitOfWork.Attendances.ListAsync();
            return _mapper.Map<IEnumerable<AttendanceResponseDto>>(list);
        }

        public async Task<AttendanceResponseDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Attendances.GetByIdAsync(id);
            if (entity == null) throw new System.Exception("Attendance not found");
            return _mapper.Map<AttendanceResponseDto>(entity);
        }

        public async Task UpdateAsync(UpdateAttendanceDto dto)
        {
            var entity = await _unitOfWork.Attendances.GetByIdAsync(dto.Id);
            if (entity == null) throw new System.Exception("Attendance not found");
            entity.AttendanceDate = dto.AttendanceDate;
            entity.IsPresent = dto.IsPresent;
            entity.UpdatedAt = System.DateTime.UtcNow;
            entity.UpdatedBy = "system";
            _unitOfWork.Attendances.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PaginatedResult<AttendanceResponseDto>> GetPagedAsync(BaseQueryParams queryParams)
        {
            var paged = await _unitOfWork.Attendances.GetPagedAsync(queryParams);
            var mapped = _mapper.Map<IEnumerable<AttendanceResponseDto>>(paged.Items);
            return new PaginatedResult<AttendanceResponseDto>(mapped, paged.TotalCount, paged.PageNumber, paged.PageSize);
        }
    }
}

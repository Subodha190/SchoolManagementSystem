using AutoMapper;
using SchoolManagement.Application.Common.Interfaces;
using SchoolManagement.Application.Common.Models;
using SchoolManagement.Application.Repositories;
using SchoolManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManagement.Application.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ApplicationUserService(IUnitOfWork unitOfWork ,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApplicationUserDto> CreateAsync(ApplicationUserDto dto)
        {
            var entity = _mapper.Map<ApplicationUser>(dto);
            entity.CreatedOn = DateTime.UtcNow;
            await _unitOfWork.ApplicationUsers.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ApplicationUserDto>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.ApplicationUsers.GetByIdAsync(id);
            if (entity == null) throw new Exception("Teacher not found");
            _unitOfWork.ApplicationUsers.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<ApplicationUserDto>> GetAllAsync()
        {
            var list = await _unitOfWork.ApplicationUsers.ListAsync();
            return _mapper.Map<IEnumerable<ApplicationUserDto>>(list);
        }

        public async Task<ApplicationUserDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.ApplicationUsers.GetByIdAsync(id);
            if (entity == null) throw new Exception("Application user not found");
            return _mapper.Map<ApplicationUserDto>(entity);
        }

        public async Task<PaginatedResult<ApplicationUserDto>> GetPagedAsync(BaseQueryParams queryParams)
        {
            var paged = await _unitOfWork.ApplicationUsers.GetPagedAsync(queryParams);
            var mapped = _mapper.Map<IEnumerable<ApplicationUserDto>>(paged.Items);
            return new PaginatedResult<ApplicationUserDto>(mapped, paged.TotalCount, paged.PageNumber, paged.PageSize);
        }

        public async Task UpdateAsync(ApplicationUserDto dto)
        {
            var entity = await _unitOfWork.ApplicationUsers.GetByIdAsync(dto.SchoolId);
            if (entity == null) throw new Exception("School not found");
            entity.FullName = dto.FullName;
            _unitOfWork.ApplicationUsers.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

using SchoolManagement.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManagement.Application.Common.Interfaces
{
    public interface IApplicationUserService
    {
        Task<ApplicationUserDto> CreateAsync(ApplicationUserDto dto);
        Task<IEnumerable<ApplicationUserDto>> GetAllAsync();
        Task<ApplicationUserDto> GetByIdAsync(int id);
        Task UpdateAsync(ApplicationUserDto dto);
        Task DeleteAsync(int id);
        Task<PaginatedResult<ApplicationUserDto>> GetPagedAsync(BaseQueryParams queryParams);
    }
}

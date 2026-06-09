using SchoolManagement.Application.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Common.Interfaces
{
    public interface ITeacherService
    {
        Task<TeacherResponseDto> CreateAsync(CreateTeacherDto dto);
        Task<IEnumerable<TeacherResponseDto>> GetAllAsync();
        Task<TeacherResponseDto> GetByIdAsync(int id);
        Task UpdateAsync(UpdateTeacherDto dto);
        Task DeleteAsync(int id);
        Task<PaginatedResult<TeacherResponseDto>> GetPagedAsync(BaseQueryParams queryParams);
    }
}

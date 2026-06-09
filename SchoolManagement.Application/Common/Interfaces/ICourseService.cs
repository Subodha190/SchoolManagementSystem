using SchoolManagement.Application.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Common.Interfaces
{
    public interface ICourseService
    {
        Task<CourseResponseDto> CreateAsync(CreateCourseDto dto);
        Task<IEnumerable<CourseResponseDto>> GetAllAsync();
        Task<CourseResponseDto> GetByIdAsync(int id);
        Task UpdateAsync(UpdateCourseDto dto);
        Task DeleteAsync(int id);
        Task<PaginatedResult<CourseResponseDto>> GetPagedAsync(BaseQueryParams queryParams);
    }
}

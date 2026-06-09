using SchoolManagement.Application.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Common.Interfaces
{
    public interface IAttendanceService
    {
        Task<AttendanceResponseDto> CreateAsync(CreateAttendanceDto dto);
        Task<IEnumerable<AttendanceResponseDto>> GetAllAsync();
        Task<AttendanceResponseDto> GetByIdAsync(int id);
        Task UpdateAsync(UpdateAttendanceDto dto);
        Task DeleteAsync(int id);
        Task<PaginatedResult<AttendanceResponseDto>> GetPagedAsync(BaseQueryParams queryParams);
    }
}

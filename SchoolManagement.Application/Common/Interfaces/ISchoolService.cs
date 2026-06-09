using SchoolManagement.Application.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Common.Interfaces
{
    public interface ISchoolService
    {
        Task<SchoolResponseDto> CreateAsync(CreateSchoolDto dto);
        Task<IEnumerable<SchoolResponseDto>> GetAllAsync();
        Task<SchoolResponseDto> GetByIdAsync(int id);
        Task UpdateAsync(UpdateSchoolDto dto);
        Task DeleteAsync(int id);
        Task<PaginatedResult<SchoolResponseDto>> GetPagedAsync(BaseQueryParams queryParams);
    }
}

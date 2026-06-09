using SchoolManagement.Application.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Common.Interfaces
{
    public interface IEnrollmentService
    {
        Task<EnrollmentResponseDto> CreateAsync(CreateEnrollmentDto dto);
        Task<IEnumerable<EnrollmentResponseDto>> GetAllAsync();
        Task<EnrollmentResponseDto> GetByIdAsync(int id);
        Task UpdateAsync(UpdateEnrollmentDto dto);
        Task DeleteAsync(int id);
        Task<PaginatedResult<EnrollmentResponseDto>> GetPagedAsync(BaseQueryParams queryParams);
    }
}

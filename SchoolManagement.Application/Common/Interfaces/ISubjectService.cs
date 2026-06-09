using SchoolManagement.Application.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Common.Interfaces
{
    public interface ISubjectService
    {
        Task<SubjectResponseDto> CreateAsync(CreateSubjectDto dto);
        Task<IEnumerable<SubjectResponseDto>> GetAllAsync();
        Task<SubjectResponseDto> GetByIdAsync(int id);
        Task UpdateAsync(UpdateSubjectDto dto);
        Task DeleteAsync(int id);
        Task<PaginatedResult<SubjectResponseDto>> GetPagedAsync(BaseQueryParams queryParams);
    }
}

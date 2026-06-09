using SchoolManagement.Application.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Common.Interfaces
{
    public interface IFeePaymentService
    {
        Task<FeePaymentResponseDto> CreateAsync(CreateFeePaymentDto dto);
        Task<IEnumerable<FeePaymentResponseDto>> GetAllAsync();
        Task<FeePaymentResponseDto> GetByIdAsync(int id);
        Task UpdateAsync(UpdateFeePaymentDto dto);
        Task DeleteAsync(int id);
        Task<PaginatedResult<FeePaymentResponseDto>> GetPagedAsync(BaseQueryParams queryParams);
    }
}

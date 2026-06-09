using SchoolManagement.Application.Common.Models;
using SchoolManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManagement.Application.Common.Interfaces
{
    public interface IStudentService
    {
        Task<StudentResponseDto> CreateAsync(CreateStudentDto dto);

        Task<IEnumerable<StudentResponseDto>> GetAllAsync();

        Task<StudentResponseDto> GetByIdAsync(int id);

        Task UpdateAsync(UpdateStudentDto dto);

        Task<PaginatedResult<StudentResponseDto>> GetStudentsAsync(
            BaseQueryParams queryParams);
    }
}

using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Application.Common.Interfaces;
using SchoolManagement.Application.Common.Models;
using SchoolManagement.API.Attributes;

namespace SchoolManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilter<CreateStudentDto>))]
        [HasPermissionAuthorize("Student.Create")]
        public async Task<IActionResult> CreateStudent(CreateStudentDto request)
        {
            var result =
                await _studentService.CreateAsync(request);

            return Ok(result);
        }

        [HttpGet]
        [HasPermissionAuthorize("Student.View")]
        public async Task<IActionResult> GetStudents(
        [FromQuery] BaseQueryParams queryParams)
        {
            var result = await _studentService
                .GetStudentsAsync(queryParams);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _studentService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(
            UpdateStudentDto dto)
        {
            await _studentService.UpdateAsync(dto);

            return Ok("Updated Successfully");
        }

    }
}

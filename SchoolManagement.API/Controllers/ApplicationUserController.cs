using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Application.Common.Interfaces;
using SchoolManagement.Application.Common.Models;
using SchoolManagement.Application.Services;

namespace SchoolManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly IApplicationUserService _applicationUser;

        public ApplicationUserController(IApplicationUserService applicationUser)
        {
            _applicationUser = applicationUser;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilter<ApplicationUserDto>))]
        public async Task<IActionResult> CreateStudent(ApplicationUserDto request)
        {
            var result =
                await _applicationUser.CreateAsync(request);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents(
        [FromQuery] BaseQueryParams queryParams)
        {
            var result = await _applicationUser
                .GetPagedAsync(queryParams);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _applicationUser.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(
            ApplicationUserDto dto)
        {
            await _applicationUser.UpdateAsync(dto);

            return Ok("Updated Successfully");
        }

    }
}

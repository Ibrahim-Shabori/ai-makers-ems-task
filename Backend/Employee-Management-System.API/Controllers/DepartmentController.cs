using Employee_Management_System.API.DTOs.Departments;
using Employee_Management_System.API.Services.IServices;
using Employee_Management_System.Infrastructure.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Management_System.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IUnitOfWork unitOfWork, IDepartmentService departmentService)
        {
            _unitOfWork = unitOfWork;
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await _departmentService.GetDepartmentsAsync();
            return Ok(departments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById([FromRoute] int id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(department);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentCreateDto departmentCreateDto)
        {
            var createdDepartment = await _departmentService.CreateDepartmentAsync(departmentCreateDto);
            return CreatedAtAction(nameof(GetDepartmentById), new { id = createdDepartment.Id }, createdDepartment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment([FromRoute] int id, [FromBody] DepartmentUpdateDto departmentUpdateDto)
        {
            await _departmentService.UpdateDepartmentAsync(id, departmentUpdateDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment([FromRoute] int id)
        {
            try
            {
                await _departmentService.DeleteDepartmentByIdAsync(id);

            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            return NoContent();
        }
}}

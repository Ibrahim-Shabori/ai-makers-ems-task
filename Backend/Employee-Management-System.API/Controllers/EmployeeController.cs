using Employee_Management_System.API.DTOs.Employees;
using Employee_Management_System.API.Services.IServices;
using Employee_Management_System.Infrastructure.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Management_System.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IUnitOfWork unitOfWork, IEmployeeService employeeService)
        {
            _unitOfWork = unitOfWork;
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees([FromQuery] EmployeeQueryParameters userParams)
        {
            var employees = await _employeeService.GetEmployeesAsync(userParams);
            return Ok(new
            {
                items = employees,
                currentPage = employees.CurrentPage,
                totalPages = employees.TotalPages,
                pageSize = employees.PageSize,
                totalCount = employees.TotalCount,
                hasPrevious = employees.HasPrevious,
                hasNext = employees.HasNext
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById([FromRoute] int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeCreateDto employeeCreateDto)
        {
            var createdEmployee = await _employeeService.CreateEmployeeAsync(employeeCreateDto);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = createdEmployee.Id }, createdEmployee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] int id, [FromBody] EmployeeUpdateDto employeeUpdateDto)
        {
            await _employeeService.UpdateEmployeeAsync(id, employeeUpdateDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
        {
            await _employeeService.DeleteEmployeeAsync(id);
            return NoContent();
        }
    }
}

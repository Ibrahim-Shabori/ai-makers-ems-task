using Employee_Management_System.API.DTOs.Employees;
using Employee_Management_System.API.Helpers;

namespace Employee_Management_System.API.Services.IServices
{
    public interface IEmployeeService
    {
        Task<PagedList<EmployeePageItemDto>> GetEmployeesAsync(EmployeeQueryParameters userParams);
        Task<EmployeeDetailsDto> CreateEmployeeAsync(EmployeeCreateDto employeeCreateDto);
        Task<EmployeeDetailsDto> GetEmployeeByIdAsync(int id);
        Task UpdateEmployeeAsync(int id, EmployeeUpdateDto employeeUpdateDto);
        Task DeleteEmployeeAsync(int id);
    }
}

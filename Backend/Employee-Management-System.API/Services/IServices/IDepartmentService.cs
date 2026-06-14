using Employee_Management_System.API.DTOs.Departments;

namespace Employee_Management_System.API.Services.IServices
{
    public interface IDepartmentService
    {
        Task<DepartmentDetailsDto> CreateDepartmentAsync(DepartmentCreateDto departmentCreateDto);
        Task<DepartmentDetailsDto> GetDepartmentByIdAsync(int id);
        Task DeleteDepartmentByIdAsync(int id);
        Task UpdateDepartmentAsync(int id, DepartmentUpdateDto departmentUpdateDto);
        Task<IEnumerable<DepartmentPageItemDto>> GetDepartmentsAsync();
    }
}

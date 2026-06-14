using Azure.Core;
using Employee_Management_System.API.DTOs.Employees;
using Employee_Management_System.API.Helpers;
using Employee_Management_System.API.Services.IServices;
using Employee_Management_System.Domain;
using Employee_Management_System.Infrastructure.Repositories.IRepositories;

namespace Employee_Management_System.API.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<EmployeeDetailsDto> CreateEmployeeAsync(EmployeeCreateDto employeeCreateDto)
        {
            var employee = new Employee
            {
                FullName = employeeCreateDto.FullName,
                JobTitle = employeeCreateDto.JobTitle,
                DepartmentId = employeeCreateDto.DepartmentId,
                Email = employeeCreateDto.Email,
                PhoneNumber = employeeCreateDto.PhoneNumber,
                HireDate = employeeCreateDto.HireDate,
                IsActive = employeeCreateDto.IsActive,
            };

            await _unitOfWork.Employee.Add(employee);
            await _unitOfWork.Save();

            return new EmployeeDetailsDto
            {
                Id = employee.Id,
                FullName = employee.FullName,
                JobTitle = employee.JobTitle,
                DepartmentName = (await _unitOfWork.Department.Get(dept => dept.Id == employee.DepartmentId)).Name,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                HireDate = employee.HireDate,
                IsActive = employee.IsActive
            };
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _unitOfWork.Employee.Get(emp => emp.Id == id, tracked: true);
            if (employee == null) return;
            _unitOfWork.Employee.Remove(employee);
            await _unitOfWork.Save();
        }

        public async Task<EmployeeDetailsDto> GetEmployeeByIdAsync(int id)
        {
            var employee = await _unitOfWork.Employee.Get(emp => emp.Id == id, IncludeProperties: "Department");
            if (employee == null) return null;

            var employeeDetailsDto = new EmployeeDetailsDto
            {
                Id = employee.Id,
                FullName = employee.FullName,
                JobTitle = employee.JobTitle,
                DepartmentName = employee.Department.Name,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                HireDate = employee.HireDate,
                IsActive = employee.IsActive
            };

            return employeeDetailsDto;
        }

        public async Task<PagedList<EmployeePageItemDto>> GetEmployeesAsync(EmployeeQueryParameters userParams)
        {
            var query = _unitOfWork.Employee.dbSet.AsQueryable();

            if (userParams.DepartmentId != null) query = query.Where(emp => emp.DepartmentId == userParams.DepartmentId);
            if (userParams.IsActive != null) query = query.Where(emp => emp.IsActive == userParams.IsActive);

            switch (userParams.SortField)
            {
                case "hireDate":
                    query = (userParams.SortOrder == 1) ?
                        query.OrderBy(emp => emp.HireDate)
                        : query.OrderByDescending(emp => emp.HireDate);
                    break;
                case "fullName":
                    query = (userParams.SortOrder == 1)
                        ? query.OrderByDescending(emp => emp.FullName)
                        : query.OrderBy(emp => emp.FullName);
                    break;
                default:
                    query = query.OrderBy(emp => emp.HireDate);
                    break;
            }

            if (userParams.SearchTerm != null && userParams.SearchTerm != "")
            {
                query = query.Where(emp => emp.FullName.ToLower().Contains(userParams.SearchTerm.ToLower()));
            }

            var dtoFormat = query.Select(emp => new EmployeePageItemDto
            {
                Id = emp.Id,
                FullName = emp.FullName,
                Email = emp.Email,
                DepartmentName = emp.Department.Name,
                JobTitle = emp.JobTitle,
                IsActive = emp.IsActive,

            });
            return await PagedList<EmployeePageItemDto>.CreateAsync(dtoFormat, userParams.PageNumber, userParams.PageSize);
        }

        public async Task UpdateEmployeeAsync(int id, EmployeeUpdateDto employeeUpdateDto)
        {
            var employee = await _unitOfWork.Employee.Get(emp => emp.Id == id, tracked: true);
            employee.FullName = employeeUpdateDto.FullName;
            employee.JobTitle = employeeUpdateDto.JobTitle;
            employee.DepartmentId = employeeUpdateDto.DepartmentId;
            employee.Email = employeeUpdateDto.Email;
            employee.PhoneNumber = employeeUpdateDto.PhoneNumber;
            employee.HireDate = employeeUpdateDto.HireDate;
            employee.IsActive = employeeUpdateDto.IsActive;
            await _unitOfWork.Save();
            return;
        }
    }
}

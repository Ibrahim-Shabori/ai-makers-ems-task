using Employee_Management_System.API.DTOs.Departments;
using Employee_Management_System.API.Services.IServices;
using Employee_Management_System.Domain;
using Employee_Management_System.Infrastructure.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management_System.API.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<DepartmentDetailsDto> CreateDepartmentAsync(DepartmentCreateDto departmentCreateDto)
        {
            var department = new Department
            {
                Name = departmentCreateDto.Name,
                Description = departmentCreateDto.Description
            };
            await _unitOfWork.Department.Add(department);
            await _unitOfWork.Save();

            return new DepartmentDetailsDto
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description
            };
        }

        public async Task DeleteDepartmentByIdAsync(int id)
        {
            var department = await _unitOfWork.Department.Get(dep => dep.Id == id, tracked: true);
            var isAnyEmployeeInDepartment = await _unitOfWork.Employee.dbSet.AnyAsync(emp => emp.DepartmentId == id);
            if (isAnyEmployeeInDepartment)
            {
                throw new InvalidOperationException("Cannot delete department because there are employees associated with it.");
            }
                if (department == null) return;
            _unitOfWork.Department.Remove(department);
            await _unitOfWork.Save();
        }

        public async Task<DepartmentDetailsDto> GetDepartmentByIdAsync(int id)
        {
            var department = await _unitOfWork.Department.Get(dep => dep.Id == id);
            if (department == null) return null;

            var departmentDetailsDto = new DepartmentDetailsDto
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description
            };

            return departmentDetailsDto;
        }

        public async Task<IEnumerable<DepartmentPageItemDto>> GetDepartmentsAsync()
        {
            var departments = await _unitOfWork.Department.GetAll();
            var departmentPageItemDtos = departments.Select(dep => new DepartmentPageItemDto
            {
                Id = dep.Id,
                Name = dep.Name,
                Description = dep.Description
            });

            return departmentPageItemDtos;
        }
        public async Task UpdateDepartmentAsync(int id, DepartmentUpdateDto departmentUpdateDto)
        {
            var department = await _unitOfWork.Department.Get(dep => dep.Id == id, tracked: true);
            if (department == null) return;

            department.Name = departmentUpdateDto.Name;
            department.Description = departmentUpdateDto.Description;

            await _unitOfWork.Save();
        }
    }
}

using Employee_Management_System.API.DTOs.Employees;
using Employee_Management_System.Domain;
using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.API.DTOs.Departments
{
    public class DepartmentDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<EmployeePageItemDto> Employees { get; set; }
    }
}

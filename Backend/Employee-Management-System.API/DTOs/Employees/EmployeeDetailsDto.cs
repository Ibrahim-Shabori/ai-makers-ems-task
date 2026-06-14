using Employee_Management_System.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee_Management_System.API.DTOs.Employees
{
    public class EmployeeDetailsDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string JobTitle { get; set; }
        public DateTime HireDate { get; set; }
        public string DepartmentName { get; set; }
        public bool IsActive { get; set; }
    }
}

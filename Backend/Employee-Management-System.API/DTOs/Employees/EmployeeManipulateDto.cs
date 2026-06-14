using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.API.DTOs.Employees
{
    public class EmployeeManipulateDto
    {
        public string FullName { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }
        [MaxLength(100, ErrorMessage = "Job title cannot exceed 100 characters.")]
        public string JobTitle { get; set; }
        public DateTime HireDate { get; set; }
        public int DepartmentId { get; set; }
        public bool IsActive { get; set; }
    }
}

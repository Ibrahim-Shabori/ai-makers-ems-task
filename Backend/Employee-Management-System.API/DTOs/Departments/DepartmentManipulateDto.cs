using Employee_Management_System.Domain;
using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.API.DTOs.Departments
{
    public class DepartmentManipulateDto
    {
        [Required(ErrorMessage = "Department name is required.")]
        public string Name { get; set; }
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

    }
}

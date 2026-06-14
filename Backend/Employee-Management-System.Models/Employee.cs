using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Employee_Management_System.Domain
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }
        [MaxLength(100, ErrorMessage = "Job title cannot exceed 100 characters.")]
        public string JobTitle { get; set; }
        public DateTime HireDate { get; set; }
        [Required(ErrorMessage = "Department is required.")]
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        public bool IsActive { get; set; } = true;
    }
}

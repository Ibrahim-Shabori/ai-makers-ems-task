using Employee_Management_System.API.DTOs.Common;

namespace Employee_Management_System.API.DTOs.Employees
{
    public class EmployeeQueryParameters : PaginationParameters
    {
        public string? SearchTerm { get; set; }
        public int? DepartmentId { get; set; }
        public string? SortField { get; set; }
        public int SortOrder { get; set; }
        public bool? IsActive { get; set; }
    }
}

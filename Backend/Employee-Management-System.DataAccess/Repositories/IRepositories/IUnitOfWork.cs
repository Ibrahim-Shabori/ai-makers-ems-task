using System;
using System.Collections.Generic;
using System.Text;

namespace Employee_Management_System.Infrastructure.Repositories.IRepositories
{
    public interface IUnitOfWork
    {
        IDepartmentRepository Department { get; }
        IEmployeeRepository Employee { get; }
        Task Save();
    }
}

using Employee_Management_System.Domain;
using Employee_Management_System.Infrastructure.Data;
using Employee_Management_System.Infrastructure.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Employee_Management_System.Infrastructure.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        private readonly ApplicationDbContext _db;
        public EmployeeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}

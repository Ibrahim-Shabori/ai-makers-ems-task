using Employee_Management_System.Infrastructure.Data;
using Employee_Management_System.Infrastructure.Repositories.IRepositories;


namespace Employee_Management_System.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public IEmployeeRepository Employee { get; private set; }
        public IDepartmentRepository Department { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Employee = new EmployeeRepository(_db);
            Department = new DepartmentRepository(_db);
        }

        public Task<int> CompleteAsync()
        {
            return Task.FromResult(0);
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}

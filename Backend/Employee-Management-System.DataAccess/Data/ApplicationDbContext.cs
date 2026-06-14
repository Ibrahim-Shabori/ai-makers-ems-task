using Employee_Management_System.Domain;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management_System.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .Property(e => e.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<Employee>()
                .HasOne(emp => emp.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(emp => emp.DepartmentId)
                .IsRequired();
        }

    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PayrollManagementSystem.Entities;

namespace PayrollManagementSystem.Context
{
    public class PayrollDbContext : IdentityDbContext<AppUser, IdentityRole<long>, long>
    {
        public PayrollDbContext(DbContextOptions<PayrollDbContext> options) : base(options)
        {
        }
        public DbSet<AppUser> AppUsers { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Department> Departments { get; set; } = null!;
        public DbSet<JobTitle> JobTitles { get; set; } = null!;
        public DbSet<EmploymentTerm> EmploymentsTerms { get; set;} = null!;
        public DbSet<WorkingHoursLog> WorkingHoursLogs { get; set; } = null!;
        public DbSet<WorkHourAdjustment> WorkHoursAdjustment { get; set;} = null!;
        public DbSet<Adjustment> Adjustments { get; set; } = null!;
        public DbSet<AdjustmentAmount> AdjustmentsAmount { get; set;} = null!;
        public DbSet<SalaryPayment> SalaryPayments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<WorkHourAdjustment>()
                    .HasOne(w => w.WorkingHoursLog)
                    .WithMany()
                    .HasForeignKey(w => w.WorkingHoursLogId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

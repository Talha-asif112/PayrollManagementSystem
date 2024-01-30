using PayrollManagementSystem.Context;
using PayrollManagementSystem.Data.Repositories.Interfaces;
using PayrollManagementSystem.Entities;

namespace PayrollManagementSystem.Data.Repositories.Implementations
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(PayrollDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }
    }
}

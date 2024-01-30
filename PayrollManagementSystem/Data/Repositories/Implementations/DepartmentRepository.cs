using PayrollManagementSystem.Context;
using PayrollManagementSystem.Data.Repositories.Interfaces;

namespace PayrollManagementSystem.Data.Repositories.Implementations
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(PayrollDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }
    }
}

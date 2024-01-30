using PayrollManagementSystem.Context;
using PayrollManagementSystem.Data.Repositories.Interfaces;
using PayrollManagementSystem.Entities;

namespace PayrollManagementSystem.Data.Repositories.Implementations
{
    public class WorkingHoursLogRepository : BaseRepository<WorkingHoursLog>, IWorkingHoursLogRepository
    {
        public WorkingHoursLogRepository(PayrollDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }
    }
}

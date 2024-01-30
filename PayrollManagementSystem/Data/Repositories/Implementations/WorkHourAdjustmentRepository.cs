using PayrollManagementSystem.Context;
using PayrollManagementSystem.Data.Repositories.Interfaces;

namespace PayrollManagementSystem.Data.Repositories.Implementations
{
    public class WorkHourAdjustmentRepository : BaseRepository<WorkHourAdjustment>, IWorkHourAdjustmentRepository
    {
        public WorkHourAdjustmentRepository(PayrollDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }
    }
}

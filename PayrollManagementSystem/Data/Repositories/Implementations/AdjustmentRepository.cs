using PayrollManagementSystem.Context;
using PayrollManagementSystem.Data.Repositories.Interfaces;

namespace PayrollManagementSystem.Data.Repositories.Implementations
{
    public class AdjustmentRepository : BaseRepository<Adjustment>, IAdjustmentRepository
    {
        public AdjustmentRepository(PayrollDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }
    }

}

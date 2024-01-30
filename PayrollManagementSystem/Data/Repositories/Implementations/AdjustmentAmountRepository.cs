using PayrollManagementSystem.Context;
using PayrollManagementSystem.Data.Repositories.Interfaces;

namespace PayrollManagementSystem.Data.Repositories.Implementations
{
    public class AdjustmentAmountRepository : BaseRepository<AdjustmentAmount>, IAdjustmentAmountRepository
    {
        public AdjustmentAmountRepository(PayrollDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }
    }
}

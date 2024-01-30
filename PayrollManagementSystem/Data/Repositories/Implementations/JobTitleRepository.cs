using PayrollManagementSystem.Context;
using PayrollManagementSystem.Data.Repositories.Interfaces;

namespace PayrollManagementSystem.Data.Repositories.Implementations
{
    public class JobTitleRepository : BaseRepository<JobTitle>, IJobTitleRepository
    {
        public JobTitleRepository(PayrollDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }
    }
}

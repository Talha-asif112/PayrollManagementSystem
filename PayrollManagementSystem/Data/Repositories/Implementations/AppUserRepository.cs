using PayrollManagementSystem.Context;
using PayrollManagementSystem.Data.Repositories.Interfaces;
using PayrollManagementSystem.Entities;

namespace PayrollManagementSystem.Data.Repositories.Implementations
{
    public class AppUserRepository : BaseRepository<AppUser>, IAppUserRepository
    {
        public AppUserRepository(PayrollDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }
    }
}

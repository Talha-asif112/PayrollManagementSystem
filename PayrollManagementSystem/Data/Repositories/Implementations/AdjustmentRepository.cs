using Microsoft.EntityFrameworkCore;
using PayrollManagementSystem.Context;
using PayrollManagementSystem.Data.Repositories.Interfaces;
using Utilities.Exceptions;

namespace PayrollManagementSystem.Data.Repositories.Implementations
{
    public class AdjustmentRepository : BaseRepository<Adjustment>, IAdjustmentRepository
    {
        public AdjustmentRepository(PayrollDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }

        public override async Task<Adjustment> Add (Adjustment entity)
        {
            await DbSet.AddAsync(entity);
            return entity;
        }

        public async Task<Adjustment> Get(long id)
        {
            var data = await DbSet.FirstOrDefaultAsync(c => !c.IsDelete && c.Id == id);
            return data;
        }

        public virtual async Task <IList<Adjustment>> GetAll()
        {
            var res = await DbSet.Where(f => f.IsDelete != true).ToListAsync();
            return res;
        }

        public virtual async Task<Adjustment> Update(Adjustment model)
        {
            var found = await DbSet
                .FirstOrDefaultAsync(c => c.Id == model.Id && c.IsDelete != true);
            if (found == null)
            {
                throw new Exception($"{nameof(Adjustment)} not found for Id:{model.Id}");
            }
            found.AdjustmentName = model.AdjustmentName;
            found.AdjustmentPercentage = model.AdjustmentPercentage;
            found.IsOtherAdjustment = model.IsOtherAdjustment;
            found.IsWorkingHoursAdjustment = model.IsWorkingHoursAdjustment;

            return found;
        }

        public override async Task<bool> Delete(long id)
        {
            var found = await DbSet.FirstOrDefaultAsync(c => c.Id == id && c.IsDelete != true);
            if (found == null)
            {
                throw new EntityNotFoundException($"{nameof(Adjustment)} not found for Id:{id}");
            }

            found.IsDelete = true;
            await DbContext.SaveChangesAsync();
            return true;
        }
    }

}

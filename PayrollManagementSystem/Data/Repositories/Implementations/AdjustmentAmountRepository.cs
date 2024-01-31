using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PayrollManagementSystem.Context;
using PayrollManagementSystem.Data.Repositories.Interfaces;
using Utilities.Exceptions;

namespace PayrollManagementSystem.Data.Repositories.Implementations
{
    public class AdjustmentAmountRepository : BaseRepository<AdjustmentAmount>, IAdjustmentAmountRepository
    {
        public AdjustmentAmountRepository(PayrollDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }
        
        public override async Task<AdjustmentAmount> Add (AdjustmentAmount entity)
        {
            await DbSet.AddAsync(entity);
            return entity;
        }

        public async Task <AdjustmentAmount> Get (long id)
        {
            var data = await DbSet.FirstOrDefaultAsync(c => !c.IsDelete && c.Id == id);
            return data;
        }

        public async Task <IList<AdjustmentAmount>> GetAll ()
        {
            var res = await DbSet.Where(f => f.IsDelete != true).ToListAsync();
            return res;
        }

        public virtual async Task<AdjustmentAmount> Update(AdjustmentAmount model)
        {
            var found = await DbSet
                .FirstOrDefaultAsync(c => c.Id == model.Id && c.IsDelete != true);
            if (found == null)
            {
                throw new Exception($"{nameof(AdjustmentAmount)} not found for Id:{model.Id}");
            }
            found.AdjustmentPercentage = model.AdjustmentPercentage;
            found.Adjustment_Amount = model.Adjustment_Amount;
            found.SalaryPaymentId = model.SalaryPaymentId;
            found.AdjustmentId = model.AdjustmentId;

            return found;
        }

        public override async Task<bool> Delete(long id)
        {
            var found = await DbSet.FirstOrDefaultAsync(c => c.Id == id && c.IsDelete != true);
            if (found == null)
            {
                throw new EntityNotFoundException($"{nameof(AdjustmentAmount)} not found for Id:{id}");
            }

            found.IsDelete = true;
            await DbContext.SaveChangesAsync();
            return true;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using PayrollManagementSystem.Context;
using PayrollManagementSystem.Data.Repositories.Interfaces;
using Utilities.Exceptions;

namespace PayrollManagementSystem.Data.Repositories.Implementations
{
    public class WorkHourAdjustmentRepository : BaseRepository<WorkHourAdjustment>, IWorkHourAdjustmentRepository
    {
        public WorkHourAdjustmentRepository(PayrollDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }

        public override async Task<WorkHourAdjustment> Add(WorkHourAdjustment model)
        {
            await DbSet.AddAsync(model);
            return model;
        }

        public async Task<WorkHourAdjustment> Get(long id)
        {
            var data = await DbSet.FirstOrDefaultAsync(c => !c.IsDelete && c.Id == id);
            return data;
        }

        public virtual async Task<IList<WorkHourAdjustment>> GetAll()
        {


            var res = await DbSet.Where(f => f.IsDelete != true).ToListAsync();
            return res;

        }

        public virtual async Task<WorkHourAdjustment> Update(WorkHourAdjustment model)
        {
            var found = await DbSet

                .FirstOrDefaultAsync(c => !c.IsDelete && c.Id == model.Id);
            if (found == null)
            {
                throw new EntityNotFoundException($"{nameof(WorkHourAdjustment)} not found with Id: {model.Id}");
            }
            found.WorkingHoursLogId = model.WorkingHoursLogId;
            found.AdjustmentAmount = model.AdjustmentAmount;
            found.AdjustmentId = model.AdjustmentId;
            found.SalaryPaymentId = model.SalaryPaymentId;
            found.AdjustmentPercentage = model.AdjustmentPercentage;


            return found;

        }

        public override async Task<bool> Delete(long id)
        {
            var found = await DbSet.FirstOrDefaultAsync(c => c.Id == id && c.IsDelete != true);
            if (found == null)
            {
                throw new EntityNotFoundException($"{nameof(WorkHourAdjustment)} not found for Id:{id}");
            }

            found.IsDelete = true;
            await DbContext.SaveChangesAsync();
            return true;
        }

    }
}

using Microsoft.EntityFrameworkCore;
using PayrollManagementSystem.Context;
using PayrollManagementSystem.Data.Repositories.Interfaces;
using PayrollManagementSystem.Entities;
using Utilities.Exceptions;

namespace PayrollManagementSystem.Data.Repositories.Implementations
{
    public class WorkingHoursLogRepository : BaseRepository<WorkingHoursLog>, IWorkingHoursLogRepository
    {
        public WorkingHoursLogRepository(PayrollDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }

        public override async Task<WorkingHoursLog> Add(WorkingHoursLog model)
        {
            await DbSet.AddAsync(model);
            return model;
        }

        public async Task<WorkingHoursLog> Get(long id)
        {
            var data = await DbSet.FirstOrDefaultAsync(c => !c.IsDelete && c.Id == id);
            return data;
        }

        public virtual async Task<IList<WorkingHoursLog>> GetAll() 
        {

            var res = await DbSet.Where(f => f.IsDelete != true).ToListAsync();
            return res;

        }

        public virtual async Task<WorkingHoursLog> Update(WorkingHoursLog model)
        {
            var found = await DbSet

                .FirstOrDefaultAsync(c => !c.IsDelete && c.Id == model.Id);
            if (found == null)
            {
                throw new EntityNotFoundException($"{nameof(WorkingHoursLog)} not found with Id: {model.Id}");
            }
            found.TimeOut = model.TimeOut;
            found.TimeIn = model.TimeIn;
            found.EmployeeId = model.EmployeeId;
            found.WorkDuration = model.WorkDuration;
            found.Date = model.Date;

            return found;

        }

        public override async Task<bool> Delete(long id)
        {
            var found = await DbSet.FirstOrDefaultAsync(c => c.Id == id && c.IsDelete != true);
            if (found == null)
            {
                throw new EntityNotFoundException($"{nameof(WorkingHoursLog)} not found for Id:{id}");
            }

            found.IsDelete = true;
            await DbContext.SaveChangesAsync();
            return true;
        }

    }
}

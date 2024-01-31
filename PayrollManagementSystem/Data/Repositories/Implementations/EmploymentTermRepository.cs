using Microsoft.EntityFrameworkCore;
using PayrollManagementSystem.Context;
using PayrollManagementSystem.Data.Repositories.Interfaces;
using System.Runtime.CompilerServices;
using Utilities.Exceptions;

namespace PayrollManagementSystem.Data.Repositories.Implementations
{
    public class EmploymentTermRepository : BaseRepository<EmploymentTerm>, IEmploymentTermRepository
    {
        public EmploymentTermRepository(PayrollDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }

        public override async Task<EmploymentTerm> Add(EmploymentTerm entity)
        {
            await DbSet.AddAsync(entity);
            return entity;
        }

        public async Task<EmploymentTerm> Get(long id)
        {
            var data = await DbSet.FirstOrDefaultAsync(c => !c.IsDelete && c.Id == id);
            return data;
        }

        public virtual async Task<IList<EmploymentTerm>> GetAll()
        {
            var res = await DbSet.Where(f => f.IsDelete != true).ToListAsync();
            return res;
        }

        public virtual async Task<EmploymentTerm> Update(EmploymentTerm model)
        {
            var found = await DbSet
                .FirstOrDefaultAsync(c => c.Id == model.Id && c.IsDelete != true);
            if (found == null)
            {
                throw new Exception($"{nameof(EmploymentTerm)} not found for Id:{model.Id}");
            }
            found.EmployeeId = model.EmployeeId;
            found.AgreedSalary = model.AgreedSalary;
            found.SalaryStartDate = model.SalaryStartDate;
            found.SalaryEndDate = model.SalaryEndDate;
            return found;
        }

        public override async Task<bool> Delete(long id)
        {
            var found = await DbSet.FirstOrDefaultAsync(c => c.Id == id && c.IsDelete != true);
            if (found == null)
            {
                throw new EntityNotFoundException($"{nameof(EmploymentTerm)} not found for Id:{id}");
            }

            found.IsDelete = true;
            await DbContext.SaveChangesAsync();
            return true;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using PayrollManagementSystem.Context;
using PayrollManagementSystem.Data.Repositories.Interfaces;
using Utilities.Exceptions;

namespace PayrollManagementSystem.Data.Repositories.Implementations
{
    public class SalaryPaymentRepository : BaseRepository<SalaryPayment>, ISalaryPaymentRepository
    {
        public SalaryPaymentRepository(PayrollDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }
        public override async Task<SalaryPayment> Add(SalaryPayment model)
        {
            await DbSet.AddAsync(model);
            return model;
        }

        public async Task<SalaryPayment> Get(long id)
        {
            var data = await DbSet.FirstOrDefaultAsync(c => !c.IsDelete && c.Id == id);
            return data;
        }

        public virtual async Task<IList<SalaryPayment>> GetAll()
        {

            var total = 0;
            var totalPages = 0;

            var res = await DbSet.Where(f => f.IsDelete != true).ToListAsync();
            return res;

        }

        public virtual async Task<SalaryPayment> Update(SalaryPayment model)
        {
            var found = await DbSet

                .FirstOrDefaultAsync(c => !c.IsDelete && c.Id == model.Id);
            if (found == null)
            {
                throw new EntityNotFoundException($"{nameof(SalaryPayment)} not found with Id: {model.Id}");
            }
            found.EmployeeId = model.EmployeeId;
            found.NetSalary = model.NetSalary;
            found.GrossSalary = model.GrossSalary;
            found.SalaryPeriod = model.SalaryPeriod;

            return found;

        }
       
        public override async Task<bool> Delete(long id)
        {
            var found = await DbSet.FirstOrDefaultAsync(c => c.Id == id && c.IsDelete != true);
            if (found == null)
            {
                throw new EntityNotFoundException($"{nameof(SalaryPayment)} not found for Id:{id}");
            }

            found.IsDelete = true;
            await DbContext.SaveChangesAsync();
            return true;
        }

    }
}

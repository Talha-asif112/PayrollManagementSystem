using Microsoft.EntityFrameworkCore;
using PayrollManagementSystem.Context;
using PayrollManagementSystem.Data.Repositories.Interfaces;
using PayrollManagementSystem.Entities;

namespace PayrollManagementSystem.Data.Repositories.Implementations
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(PayrollDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }
        public override async Task<Employee> Add(Employee entity)
        {
            await DbSet.AddAsync(entity);
            return entity;
        }

        public async Task<Employee> Get(long id)
        {
            var data = await DbSet.FirstOrDefaultAsync(c=>!c.IsDelete && c.Id == id);
            return data;
        }

        public virtual async Task<IList<Employee>> GetAll()
        {
            var res = await DbSet.Where(f => f.IsDelete != true).ToListAsync();
            return res;
        }

        public virtual async Task<Employee> Update(Employee model)
        {
            var found = await DbSet
                .FirstOrDefaultAsync(c => c.Id == model.Id && c.IsDelete != true);
            if (found == null)
            {
                throw new Exception($"{nameof(Employee)} not found for Id:{model.Id}");
            }
            found.EmploymentStart = model.EmploymentStart;
            found.Address = model.Address;
            found.DateOfBirth = model.DateOfBirth;
            found.AppUserId = model.AppUserId;
            found.DepartmentId = model.DepartmentId;
            found.JobTitleId = model.JobTitleId;   

            return found;
        }
    }
}

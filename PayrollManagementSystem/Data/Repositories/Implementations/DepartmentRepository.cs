using Microsoft.EntityFrameworkCore;
using PayrollManagementSystem.Context;
using PayrollManagementSystem.Data.Repositories.Interfaces;
using System.Reflection.Metadata.Ecma335;
using Utilities.Exceptions;

namespace PayrollManagementSystem.Data.Repositories.Implementations
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(PayrollDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }

        public override async Task<Department> Add(Department entity)
        {
            await DbSet.AddAsync(entity);
            return entity;
        }

        public async Task<Department> Get(long id)
        {
            var data = await DbSet.FirstOrDefaultAsync(c => !c.IsDelete && c.Id == id);
            return data;
        }
        public virtual async Task <IList<Department>> GetAll()
        {
            var res = await DbSet.Where(f => f.IsDelete != true).ToListAsync();
            return res;
        }

        public virtual async Task<Department> Update(Department model)
        {
            var found = await DbSet
                .FirstOrDefaultAsync(c => c.Id == model.Id && c.IsDelete != true);
            if (found == null)
            {
                throw new Exception($"{nameof(Department)} not found for Id:{model.Id}");
            }
            found.DepartmentName = model.DepartmentName;
            return found;

        }
        public override async Task<bool> Delete(long id)
        {
            var found = await DbSet.FirstOrDefaultAsync(c => c.Id == id && c.IsDelete != true);
            if (found == null)
            {
                throw new EntityNotFoundException($"{nameof(Department)} not found for Id:{id}");
            }

            found.IsDelete = true;
            await DbContext.SaveChangesAsync();
            return true;
        }
    }
}

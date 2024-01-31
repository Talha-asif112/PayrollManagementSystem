using Microsoft.EntityFrameworkCore;
using PayrollManagementSystem.Context;
using PayrollManagementSystem.Data.Repositories.Interfaces;
using Utilities.Exceptions;

namespace PayrollManagementSystem.Data.Repositories.Implementations
{
    public class JobTitleRepository : BaseRepository<JobTitle>, IJobTitleRepository
    {
        public JobTitleRepository(PayrollDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }
        public override async Task<JobTitle> Add(JobTitle model)
        {
            await DbSet.AddAsync(model);
            return model;
        }

        public async Task<JobTitle> Get(long id)
        {
            var data = await DbSet.FirstOrDefaultAsync(c => !c.IsDelete && c.Id == id);
            return data;
        }

        public virtual async Task<IList<JobTitle>> GetAll()
        {

            var total = 0;
            var totalPages = 0;

            var res = await DbSet.Where(f => f.IsDelete != true).ToListAsync();

            return res;

        }

        public virtual async Task<JobTitle> Update(JobTitle model)
        {
            var found = await DbSet

                .FirstOrDefaultAsync(c => !c.IsDelete && c.Id == model.Id);
            if (found == null)
            {
                throw new EntityNotFoundException($"{nameof(JobTitle)} not found with Id: {model.Id}");
            }
            found.Title = model.Title;
            return found;

        }

        public override async Task<bool> Delete(long id)
        {
            var found = await DbSet.FirstOrDefaultAsync(c => c.Id == id && c.IsDelete != true);
            if (found == null)
            {
                throw new EntityNotFoundException($"{nameof(JobTitle)} not found for Id:{id}");
            }

            found.IsDelete = true;
            await DbContext.SaveChangesAsync();
            return true;
        }


    }
}

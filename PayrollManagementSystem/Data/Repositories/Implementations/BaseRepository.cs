using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PayrollManagementSystem.Context;
using PayrollManagementSystem.Entities.Base;


namespace PayrollManagementSystem.Data.Repositories.Implementations;

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>

    where TEntity : class, IGeneralBase
{
    protected readonly DbSet<TEntity> DbSet;
    protected readonly PayrollDbContext DbContext;
    public readonly HttpContext HttpContext;

    protected BaseRepository(PayrollDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        DbSet = dbContext.Set<TEntity>();
        DbContext = dbContext;
        HttpContext = httpContextAccessor.HttpContext ??
                      throw new NotImplementedException(
                          "HttpContextAccessor Cannot be Null. Verify whether it's properly Injected or not.");
    }

    public virtual async Task<TEntity> Add(TEntity model)
    {
        await DbSet.AddAsync(model);
        return model;
    }

    public virtual async Task<bool> Delete(long id)
    {
        var found = await DbSet
            .FirstOrDefaultAsync(c => c.Id == id && c.IsDelete != true);
        if (found == null)
        {
            throw new Exception($"{nameof(TEntity)} not found for Id:{id}");
        }

        found.IsDelete = true;
        return true;
    }

    public virtual async Task<TEntity> Get(long id)
    {
        var data = await DbSet.FirstOrDefaultAsync(c => !c.IsDelete && c.Id == id);
        return data?.Adapt<TEntity>() ?? throw new InvalidOperationException();
    }

    public virtual async Task<IList<TEntity>> GetAll()
    {
        var res = await DbSet.Where(f => f.IsDelete != true).ToListAsync();
        return res;
    }


    public virtual async Task<TEntity> Update(TEntity model, Func<TEntity, TEntity>? func)
    {
        var found = await DbSet
            .FirstOrDefaultAsync(c => !c.IsDelete && c.Id == model.Id);
        if (found == null)
        {
            throw new Exception($"{nameof(TEntity)} not found with Id: {model.Id}");
        }

        if (func != null)
            found = func(found);

        return found;
    }

    public void Dispose()
    {
        DbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}
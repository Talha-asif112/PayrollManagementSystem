using Microsoft.AspNetCore.Mvc;
using PayrollManagementSystem.Business;
using PayrollManagementSystem.Data.UnitOfWork;
using PayrollManagementSystem.Entities.Base;
using TimeWise.Business.Interfaces;
using TimeWise.Domain.Interfaces;


namespace TimeWise.Business.Implementations;

public class BaseService<TReq, TRes, TRepository, T> : IBaseService<TReq, TRes>
    where TRepository : class, IBaseRepository<T> where T : IMinBase
{
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly TRepository Repository;

    protected BaseService(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
        Repository = UnitOfWork.GetRepository<TRepository>();
    }

    public virtual async Task<IActionResult> GetAll()
    {
        try
        {
            var data = await Repository.GetAll();
            return data.Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            // TODO: Log this I guess? or maybe return it?
            return e.GetBaseException().Message.BadRequest();
        }
    }

    public virtual async Task<IActionResult> Get(long id)
    {
        try
        {
            var entity = await Repository.Get(id);
            return entity.Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return e.HandleError();
        }
    }

    public virtual async Task<IActionResult> Add(TReq reqModel)
    {
        try
        {
            var ss = await Repository.Add((T)(reqModel as IGeneralBase ??
                                                throw new InvalidOperationException(
                                                    "Conversion to IMinBase Failed. Make sure there's Id and CreatedDate properties.")));
            return ss.Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return e.HandleError();
        }
    }

    public virtual async Task<IActionResult> Update(TReq reqModel)
    {
        try
        {
            var res = await Repository.Update((T)(reqModel as IMinBase ??
                                                    throw new InvalidOperationException(
                                                        "Conversion to IMinBase Failed. Make sure there's Id and CreatedDate properties.")),
                null);

            return res.Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return e.HandleError();
        }
    }

    public virtual async Task<IActionResult> Delete(long id)
    {
        try
        {
            var res = await Repository.Delete(id);
            await UnitOfWork.Context.SaveChangesAsync();
            return res.Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return e.HandleError();
        }
    }
}
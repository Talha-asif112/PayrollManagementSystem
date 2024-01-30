using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayrollManagementSystem.Business.Dtos.Request;
using PayrollManagementSystem.Business.Dtos.Response;
using PayrollManagementSystem.Business.Interfaces;
using PayrollManagementSystem.Data.Repositories.Implementations;
using PayrollManagementSystem.Data.UnitOfWork;
using TimeWise.Business.Implementations;
using TimeWise.Business.Interfaces;
using TimeWise.Domain.Interfaces;

namespace PayrollManagementSystem.Business.Implementations
{
    public class JobTitleService : BaseService<JobTitleReq, JobTitleRes, JobTitleRepository, JobTitle>, IJobTitleService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public JobTitleService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor) : base(unitOfWork)
        {
            _contextAccessor = contextAccessor;
        }

        public override async Task<IActionResult> GetAll()
        {
            try
            {
                var jobTitle = new List<JobTitleRes>();
                var (role, userId) = _contextAccessor.HttpContext.GetRoleAndId();
                var data = await Repository.GetAll();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        jobTitle.Add(new JobTitleRes
                        {
                            Id = item.Id,
                            Title = item.Title,
                        });
                    }
                }
                return jobTitle.Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return e.HandleError().BadRequest();
            }

        }

        public override async Task<IActionResult> Get(long id)
        {
            try
            {
                var jobTitle = new JobTitleRes();
                var (role, userId) = _contextAccessor.HttpContext.GetRoleAndId();
                var data = await Repository.Get(id);
                if (data != null)
                {
                    jobTitle = new JobTitleRes
                    {
                        Id = data.Id,
                        Title = data.Title
                    };
                }
                return jobTitle.Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return e.HandleError().BadRequest();
            }

        }

        public override async Task<IActionResult> Add(JobTitleReq reqModel)
        {
            var transaction = await UnitOfWork.BeginTransactionAsync();
            try
            {
                var (role, userId) = _contextAccessor.HttpContext.GetRoleAndId();
                JobTitle jobTitle = new JobTitle
                {
                    Title = reqModel.Title,
                    CreatedDate = DateTime.UtcNow,
                    CreatedById = userId,
                    IsActive = true
                };

                var existingJobTitle = await UnitOfWork.Context.JobTitles.FirstOrDefaultAsync(c => c.Title == reqModel.Title && c.IsDelete != true);
                if (existingJobTitle != null)
                {
                    return ("Job Title Already exist with the same Name.").BadRequest();
                }

                var ss = await Repository.Add(jobTitle);
                await UnitOfWork.SaveAsync();
                await UnitOfWork.CommitTransactionAsync(transaction);
                return ss.Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await UnitOfWork.RollBackTransactionAsync(transaction);
                return e.HandleError().BadRequest();
            }
        }

        public override async Task<IActionResult> Update(JobTitleReq reqModel)
        {
            var transaction = await UnitOfWork.BeginTransactionAsync();
            try
            {
                var (role, userId) = _contextAccessor.HttpContext.GetRoleAndId();
                var res = await Repository.Update((JobTitle)(reqModel as IMinBase ??
                        throw new InvalidOperationException(
                        "Conversion to IMinBase Failed. Make sure there's Id and CreatedDate properties.")),
                        (jobTitle) =>
                        {
                        jobTitle.Title = reqModel.Title;
                        jobTitle.ModifiedDate = DateTime.UtcNow;
                        jobTitle.ModifiedById = userId;
                        return jobTitle;
                         });

                await UnitOfWork.SaveAsync();
                await UnitOfWork.CommitTransactionAsync(transaction);
                return res.Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await UnitOfWork.RollBackTransactionAsync(transaction);
                return e.HandleError().BadRequest();
            }
        }

        public override async Task<IActionResult> Delete(long id)
        {
            var transaction = await UnitOfWork.BeginTransactionAsync();
            try
            {
                var (role, userId) = _contextAccessor.HttpContext.GetRoleAndId();
                var res = await Repository.Update((JobTitle)(new JobTitleReq() as IMinBase ??
                    throw new InvalidOperationException(
                    "Conversion to IMinBase Failed. Make sure there's Id and CreatedDate properties.")),
                    (jobTitle) =>
                      {
                        jobTitle.IsDelete = true;
                        jobTitle.ModifiedDate = DateTime.UtcNow;
                        jobTitle.ModifiedById = userId;
                        return jobTitle;
                    });

                await UnitOfWork.SaveAsync();
                await UnitOfWork.CommitTransactionAsync(transaction);
                return res.Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await UnitOfWork.RollBackTransactionAsync(transaction);
                return e.HandleError().BadRequest();
            }


        }
    }
}

using Microsoft.AspNetCore.Mvc;
using PayrollManagementSystem.Business.Dtos.Request;
using PayrollManagementSystem.Business.Dtos.Response;
using PayrollManagementSystem.Business.Interfaces;
using PayrollManagementSystem.Data.Repositories.Implementations;
using PayrollManagementSystem.Data.UnitOfWork;
using PayrollManagementSystem.Entities;
using TimeWise.Business.Implementations;

namespace PayrollManagementSystem.Business.Implementations
{
    public class WorkHourAdjustmentService : BaseService<
        WorkHourAdjustmentReq,
        WorkHourAdjustmentRes,
        WorkHourAdjustmentRepository,
        WorkHourAdjustment
        >,
        IWorkHourAdjustmentService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public WorkHourAdjustmentService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor) : base(unitOfWork)
        {
            _contextAccessor = contextAccessor;
        }

        public override async Task<IActionResult> Add(WorkHourAdjustmentReq reqModel)
        {
            var transaction = await UnitOfWork.BeginTransactionAsync();

            try
            {
                var (role, userId) = _contextAccessor.HttpContext.GetRoleAndId();
                var user = await UnitOfWork.Context.Users.FindAsync(userId);
                if (user == null)
                {
                    return "User not found".BadRequest();
                }

                var workHourAdjustment = new WorkHourAdjustment
                {
                    WorkingHoursLogId = reqModel.WorkingHoursLogId,
                    AdjustmentId = reqModel.AdjustmentId,
                    SalaryPaymentId = reqModel.SalaryPaymentId,
                    AdjustmentAmount = reqModel.AdjustmentAmount,
                    AdjustmentPercentage = reqModel.AdjustmentPercentage
                };

                await Repository.Add(workHourAdjustment);
                await UnitOfWork.SaveAsync();
                var workHourAdjustmentRes = new WorkHourAdjustmentRes
                {
                    Id = workHourAdjustment.Id,
                    WorkingHoursLogId = workHourAdjustment.WorkingHoursLogId,
                    AdjustmentId = workHourAdjustment.AdjustmentId,
                    SalaryPaymentId = workHourAdjustment.SalaryPaymentId,
                    AdjustmentAmount = workHourAdjustment.AdjustmentAmount,
                    AdjustmentPercentage = workHourAdjustment.AdjustmentPercentage
                };

                await UnitOfWork.CommitTransactionAsync(transaction);

                return workHourAdjustmentRes.Ok();
            }
            catch (Exception e)
            {
                await UnitOfWork.RollBackTransactionAsync(transaction);
                return e.Message.BadRequest();
            }
        }

        public async Task<IActionResult> Update(long id, WorkHourAdjustmentReq reqModel)
        {
            var transaction = await UnitOfWork.BeginTransactionAsync();

            try
            {
                var (role, userId) = _contextAccessor.HttpContext.GetRoleAndId();
                var user = await UnitOfWork.Context.Users.FindAsync(userId);
                if (user == null)
                {
                    return "User not found".BadRequest();
                }

                var workHourAdjustment = UnitOfWork.Context.WorkHoursAdjustment.Find(id);
                if (workHourAdjustment == null)
                {
                    return "WorkHourAdjustment not found".BadRequest();
                }

                workHourAdjustment.WorkingHoursLogId = reqModel.WorkingHoursLogId;
                workHourAdjustment.AdjustmentId = reqModel.AdjustmentId;
                workHourAdjustment.SalaryPaymentId = reqModel.SalaryPaymentId;
                workHourAdjustment.AdjustmentAmount = reqModel.AdjustmentAmount;
                workHourAdjustment.AdjustmentPercentage = reqModel.AdjustmentPercentage;

                await Repository.Update(workHourAdjustment, null);
                await UnitOfWork.SaveAsync();
                var workHourAdjustmentRes = new WorkHourAdjustmentRes
                {
                    Id = workHourAdjustment.Id,
                    WorkingHoursLogId = workHourAdjustment.WorkingHoursLogId,
                    AdjustmentId = workHourAdjustment.AdjustmentId,
                    SalaryPaymentId = workHourAdjustment.SalaryPaymentId,
                    AdjustmentAmount = workHourAdjustment.AdjustmentAmount,
                    AdjustmentPercentage = workHourAdjustment.AdjustmentPercentage
                };

                await UnitOfWork.CommitTransactionAsync(transaction);

                return workHourAdjustmentRes.Ok();
            }
            catch (Exception e)
            {
                await UnitOfWork.RollBackTransactionAsync(transaction);
                return e.Message.BadRequest();
            }
        }

        public override async Task<IActionResult> Delete(long id)
        {
            var transaction = await UnitOfWork.BeginTransactionAsync();

            try
            {
                var (role, userId) = _contextAccessor.HttpContext.GetRoleAndId();
                var user = await UnitOfWork.Context.Users.FindAsync(userId);
                if (user == null)
                {
                    return "User not found".BadRequest();
                }

                var workHourAdjustment = UnitOfWork.Context.WorkHoursAdjustment.Find(id);
                if (workHourAdjustment == null)
                {
                    return "WorkHourAdjustment not found".BadRequest();
                }

                await Repository.Delete(workHourAdjustment.Id);
                await UnitOfWork.SaveAsync();

                await UnitOfWork.CommitTransactionAsync(transaction);

                return "WorkHourAdjustment deleted successfully".Ok();
            }
            catch (Exception e)
            {
                await UnitOfWork.RollBackTransactionAsync(transaction);
                return e.Message.BadRequest();
            }
        }

        public override async Task<IActionResult> Get(long id)
        {
            try
            {
                var workHourAdjustment = await Repository.Get(id);
                if (workHourAdjustment == null)
                {
                    return "WorkHourAdjustment not found".BadRequest();
                }

                var workHourAdjustmentRes = new WorkHourAdjustmentRes
                {
                    Id = workHourAdjustment.Id,
                    WorkingHoursLogId = workHourAdjustment.WorkingHoursLogId,
                    AdjustmentId = workHourAdjustment.AdjustmentId,
                    SalaryPaymentId = workHourAdjustment.SalaryPaymentId,
                    AdjustmentAmount = workHourAdjustment.AdjustmentAmount,
                    AdjustmentPercentage = workHourAdjustment.AdjustmentPercentage
                };

                return workHourAdjustmentRes.Ok();
            }
            catch (Exception e)
            {
                return e.Message.BadRequest();
            }
        }

        public override async Task<IActionResult> GetAll()
        {
            try
            {
                var workHourAdjustments = await Repository.GetAll();
                if (workHourAdjustments == null)
                {
                    return "WorkHourAdjustments not found".BadRequest();
                }

                var workHourAdjustmentResList = new List<WorkHourAdjustmentRes>();
                foreach (var workHourAdjustment in workHourAdjustments)
                {
                    var workHourAdjustmentRes = new WorkHourAdjustmentRes
                    {
                        Id = workHourAdjustment.Id,
                        WorkingHoursLogId = workHourAdjustment.WorkingHoursLogId,
                        AdjustmentId = workHourAdjustment.AdjustmentId,
                        SalaryPaymentId = workHourAdjustment.SalaryPaymentId,
                        AdjustmentAmount = workHourAdjustment.AdjustmentAmount,
                        AdjustmentPercentage = workHourAdjustment.AdjustmentPercentage
                    };

                    workHourAdjustmentResList.Add(workHourAdjustmentRes);
                }

                return workHourAdjustmentResList.Ok();
            }
            catch (Exception e)
            {
                return e.Message.BadRequest();
            }
        }


    }
}

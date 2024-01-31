using Microsoft.AspNetCore.Mvc;
using PayrollManagementSystem.Business.Dtos.Request;
using PayrollManagementSystem.Business.Dtos.Response;
using PayrollManagementSystem.Business.Interfaces;
using PayrollManagementSystem.Data.Repositories.Implementations;
using PayrollManagementSystem.Data.UnitOfWork;
using System.Linq;
using TimeWise.Business.Implementations;
using TimeWise.Business.Interfaces;

namespace PayrollManagementSystem.Business.Implementations
{
    public class AdjustmentService : BaseService<AdjustmentReq, AdjustmentRes, AdjustmentRepository, Adjustment>, IAdjustmentService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public AdjustmentService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor) : base(unitOfWork)
        {
            _contextAccessor = contextAccessor;
        }

        public override async Task<IActionResult> Add(AdjustmentReq reqModel)
        {
            var transaction = await UnitOfWork.BeginTransactionAsync();

            try
            {
                var (role, userId) = _contextAccessor.HttpContext.GetRoleAndId();
                var user = await UnitOfWork.Context.Users.FindAsync(userId);

                var adjustment = new Adjustment
                {
                    AdjustmentName = reqModel.AdjustmentName,
                    AdjustmentPercentage = reqModel.AdjustmentPercentage,
                    IsWorkingHoursAdjustment = reqModel.IsWorkingHoursAdjustment? true : false,
                    IsOtherAdjustment = reqModel.IsOtherAdjustment?true : false,
                };

                await Repository.Add(adjustment);
                await UnitOfWork.SaveAsync();
                var adjustmentRes = new AdjustmentRes
                {
                    Id = adjustment.Id,
                    AdjustmentName = adjustment.AdjustmentName,
                    AdjustmentPercentage = adjustment.AdjustmentPercentage,
                };

                await UnitOfWork.CommitTransactionAsync(transaction);

                return adjustmentRes.Ok();
            }
            catch (Exception e)
            {
                await UnitOfWork.RollBackTransactionAsync(transaction);
                return e.Message.BadRequest();
            }
        }
        
        public override async Task<IActionResult> Update(AdjustmentReq reqModel)
        {
            var transaction = await UnitOfWork.BeginTransactionAsync();

            try
            {
                var (role, userId) = _contextAccessor.HttpContext.GetRoleAndId();
                var user = await UnitOfWork.Context.Users.FindAsync(userId);
                var adjustment = UnitOfWork.Context.Adjustments.FirstOrDefault(x=> x.Id == reqModel.Id);
                if (adjustment == null)
                {
                    return "Adjustment not found".BadRequest();
                }

                adjustment.AdjustmentName = reqModel.AdjustmentName;
                adjustment.AdjustmentPercentage = reqModel.AdjustmentPercentage;
                adjustment.IsWorkingHoursAdjustment = reqModel.IsWorkingHoursAdjustment ? true : false;
                adjustment.IsOtherAdjustment = reqModel.IsOtherAdjustment ? true : false;

                await Repository.Update(adjustment);
                await UnitOfWork.SaveAsync();
                var adjustmentRes = new AdjustmentRes
                {
                    Id = adjustment.Id,
                    AdjustmentName = adjustment.AdjustmentName,
                    AdjustmentPercentage = adjustment.AdjustmentPercentage,
                };

                await UnitOfWork.CommitTransactionAsync(transaction);

                return adjustmentRes.Ok();
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
                var adjustment = UnitOfWork.Context.Adjustments.FirstOrDefault(x=> x.Id == id);
                if (adjustment == null)
                {
                    return "Adjustment not found".BadRequest();
                }

                await Repository.Delete(id);
                await UnitOfWork.SaveAsync();

                await UnitOfWork.CommitTransactionAsync(transaction);

                return "Adjustment deleted successfully".Ok();
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
                var adjustment = new AdjustmentRes();
                var (role, userId) = _contextAccessor.HttpContext.GetRoleAndId();
                var data = UnitOfWork.Context.Adjustments.FirstOrDefault(x=> x.Id == id);
                adjustment = new AdjustmentRes
                {
                    Id = data.Id,
                    AdjustmentName = data.AdjustmentName,
                    AdjustmentPercentage = data.AdjustmentPercentage,
                };

                return adjustment.Ok();
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
                var adjustments = new List<AdjustmentRes>();
                var (role, userId) = _contextAccessor.HttpContext.GetRoleAndId();
                var data = await Repository.GetAll();
                adjustments = data.Select(x => new AdjustmentRes
                {
                    Id = x.Id,
                    AdjustmentName = x.AdjustmentName,
                    AdjustmentPercentage = x.AdjustmentPercentage,
                }).ToList();

                return adjustments.Ok();
            }
            catch (Exception e)
            {
                return e.Message.BadRequest();
            }
        }

    }
}

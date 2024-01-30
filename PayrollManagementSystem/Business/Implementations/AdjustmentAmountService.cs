using PayrollManagementSystem.Business.Dtos.Request;
using PayrollManagementSystem.Business.Dtos.Response;
using PayrollManagementSystem.Business.Interfaces;
using PayrollManagementSystem.Data.Repositories.Implementations;
using PayrollManagementSystem.Data.UnitOfWork;
using TimeWise.Business.Implementations;

namespace PayrollManagementSystem.Business.Implementations
{
    public class AdjustmentAmountService : BaseService<AdjustmentAmountReq, AdjustmentRes, AdjustmentAmountRepository, AdjustmentAmount>, IAdjustmentAmountService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public AdjustmentAmountService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor) : base(unitOfWork)
        {
            _contextAccessor = contextAccessor;
        }
    }
}

using PayrollManagementSystem.Business.Dtos.Request;
using PayrollManagementSystem.Business.Dtos.Response;
using TimeWise.Business.Interfaces;

namespace PayrollManagementSystem.Business.Interfaces
{
    public interface IAdjustmentAmountService : IBaseService<AdjustmentAmountReq, DepartmentRes>
    {
    }
}

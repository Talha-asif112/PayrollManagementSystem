using Microsoft.AspNetCore.Mvc;
using PayrollManagementSystem.Business.Dtos.Request;
using PayrollManagementSystem.Business.Dtos.Response;
using PayrollManagementSystem.Business.Interfaces;
using TimeWise.API.Helpers;

namespace PayrollManagementSystem.Controllers
{
    public class AdjustmentController : BaseController<AdjustmentController, IAdjustmentService, AdjustmentReq, AdjustmentRes>
    {
        public AdjustmentController(ILogger<AdjustmentController> logger, IAdjustmentService service) : base(logger, service)
        {
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayrollManagementSystem.Business.Dtos.Request;
using PayrollManagementSystem.Business.Dtos.Response;
using PayrollManagementSystem.Business.Interfaces;
using TimeWise.API.Helpers;

namespace PayrollManagementSystem.Controllers
{
    public class WorkingHourLogController : BaseController<WorkingHourLogController, IWorkingHourLogService, WorkingHourLogReq, WorkingHourLogRes>
    {
        public WorkingHourLogController(ILogger<WorkingHourLogController> logger, IWorkingHourLogService service) : base(logger, service)
        {
        }
        [HttpPost("CheckIn")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> CheckIn()
        {
            var res = await Service.CheckIn();
            return res;

        }
        [HttpPost("CheckOut")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> CheckOut()
        {
            var res = await Service.CheckOut();
            return res;
        }
    }

}

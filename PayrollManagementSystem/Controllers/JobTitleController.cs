using Microsoft.AspNetCore.Mvc;
using PayrollManagementSystem.Business.Dtos.Request;
using PayrollManagementSystem.Business.Dtos.Response;
using PayrollManagementSystem.Business.Interfaces;
using TimeWise.API.Helpers;

namespace PayrollManagementSystem.Controllers
{
    public class JobTitleController : BaseController<JobTitleController, IJobTitleService, JobTitleReq, JobTitleRes>
    {
        public JobTitleController(ILogger<JobTitleController> logger, IJobTitleService service) : base(logger, service)
        {
        }
    }

}

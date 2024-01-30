using Microsoft.AspNetCore.Mvc;
using PayrollManagementSystem.Business.Dtos.Request;
using PayrollManagementSystem.Business.Dtos.Response;
using PayrollManagementSystem.Business.Interfaces;
using TimeWise.API.Helpers;

namespace PayrollManagementSystem.Controllers
{
    public class EmployeeController : BaseController<EmployeeController, IEmployeeService, EmployeeReq, EmployeeRes>
    {
        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService service) : base(logger, service)
        {
        }
    }
}

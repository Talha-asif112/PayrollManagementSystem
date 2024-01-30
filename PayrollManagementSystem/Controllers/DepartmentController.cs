using Microsoft.AspNetCore.Mvc;
using PayrollManagementSystem.Business.Dtos.Request;
using PayrollManagementSystem.Business.Dtos.Response;
using PayrollManagementSystem.Business.Interfaces;
using TimeWise.API.Helpers;

namespace PayrollManagementSystem.Controllers
{
    public class DepartmentController : BaseController<DepartmentController, IDepartmentService, DepartmentReq, DepartmentRes>
    {
        public DepartmentController(ILogger<DepartmentController> logger, IDepartmentService service) : base(logger, service)
        {
        }
    }
}

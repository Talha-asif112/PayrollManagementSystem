using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PayrollManagementSystem.Business.Dtos.Request;
using PayrollManagementSystem.Business.Dtos.Response;
using PayrollManagementSystem.Business.Interfaces;
using TimeWise.API.Helpers;

namespace PayrollManagementSystem.Controllers
{
    public class AppUserController : BaseController<AppUserController, IAppUserService, AppUserReq, AppUserRes>
    {
        public AppUserController(ILogger<AppUserController> logger, IAppUserService appUserService) : base(logger, appUserService)
        {

        }

        [HttpPost("Login")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginReq login)
        {
            var res = await Service.LoginUser(login);
            return res;
        }

    }
}

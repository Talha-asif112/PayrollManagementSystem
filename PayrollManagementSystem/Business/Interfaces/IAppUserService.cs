using Microsoft.AspNetCore.Mvc;
using PayrollManagementSystem.Business.Dtos.Request;
using PayrollManagementSystem.Business.Dtos.Response;
using PayrollManagementSystem.Entities;
using TimeWise.Business.Interfaces;

namespace PayrollManagementSystem.Business.Interfaces
{
    public interface IAppUserService: IBaseService<AppUserReq, AppUserRes>
    {
        Task<IActionResult> LoginUser(LoginReq req);

    }

    public class LoginReq
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using PayrollManagementSystem.Business.Dtos.Request;
using PayrollManagementSystem.Business.Dtos.Response;
using PayrollManagementSystem.Controllers;
using TimeWise.Business.Interfaces;

namespace PayrollManagementSystem.Business.Interfaces
{
    public interface IWorkingHourLogService : IBaseService <WorkingHourLogReq, WorkingHourLogRes>
    {
        Task <IActionResult> CheckIn() ;
        Task <IActionResult> CheckOut() ;
    }
}

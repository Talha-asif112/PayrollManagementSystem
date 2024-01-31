using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayrollManagementSystem.Business.Dtos.Request;
using PayrollManagementSystem.Business.Dtos.Response;
using PayrollManagementSystem.Business.Interfaces;
using PayrollManagementSystem.Controllers;
using PayrollManagementSystem.Data.Repositories.Implementations;
using PayrollManagementSystem.Data.UnitOfWork;
using PayrollManagementSystem.Entities;
using System.Linq;
using TimeWise.Business.Implementations;

namespace PayrollManagementSystem.Business.Implementations
{
    public class WorkingHourLogService : BaseService<WorkingHourLogReq, WorkingHourLogRes, WorkingHoursLogRepository, WorkingHoursLog>, IWorkingHourLogService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public WorkingHourLogService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor) : base(unitOfWork)
        {
            _contextAccessor = contextAccessor;
        }

        public async Task<IActionResult> CheckIn()
        {
            var transaction = await UnitOfWork.BeginTransactionAsync();

            try
            {
                var (role, userId) = _contextAccessor.HttpContext.GetRoleAndId();
                DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Now);
                var user = await UnitOfWork.Context.Users.FindAsync(userId);
                var employeeId = await UnitOfWork.Context.Employees.Where(x => x.AppUserId == userId).Select(x => x.Id).FirstOrDefaultAsync();

                var employee = await UnitOfWork.Context.Employees.FindAsync(employeeId);
                var sameCheckInDay = await UnitOfWork.Context.WorkingHoursLogs.FirstOrDefaultAsync(
                    x => x.EmployeeId == employeeId &&
                    DateOnly.FromDateTime((DateTime)x.Date) == DateOnly.FromDateTime(DateTime.Now));

                if (sameCheckInDay != null)
                    return "You have already checked in today".BadRequest();

                var workingHourLog = new WorkingHoursLog
                {
                    Employee = employee,
                    Date = DateTime.Now,
                    TimeIn = DateTime.Now,
                    TimeOut = null,
                    WorkDuration = null,
                    CreatedById = userId,
                    
                };

                await Repository.Add(workingHourLog);
                await UnitOfWork.SaveAsync();
                var workingHourLogRes = new WorkingHourLogRes
                {
                    EmployeeId = workingHourLog.EmployeeId,
                    Date = workingHourLog.Date,
                    TimeIn = workingHourLog.TimeIn,
                    TimeOut = workingHourLog.TimeOut,
                    WorkDuration = workingHourLog.WorkDuration
                };

                await UnitOfWork.CommitTransactionAsync(transaction);

                return workingHourLogRes.Ok();
            }
            catch (Exception e)
            {
                await UnitOfWork.RollBackTransactionAsync(transaction);
                return e.Message.BadRequest();
            }
        }

        public async Task<IActionResult> CheckOut()
        {
            var transaction = await UnitOfWork.BeginTransactionAsync();

            try
            {
                var (role, userId) = _contextAccessor.HttpContext.GetRoleAndId();
                var employeeId = await UnitOfWork.Context.Employees.Where(x => x.AppUserId == userId).Select(x => x.Id).FirstOrDefaultAsync();
                var employee = await UnitOfWork.Context.Employees.FindAsync(employeeId);
                if (employee == null)
                {
                    return "Employee not found".BadRequest();
                }
                var workingHourLog = await UnitOfWork.Context.WorkingHoursLogs.Where(x=>x.EmployeeId == employee.Id).FirstOrDefaultAsync();
                if (workingHourLog == null)
                {
                    return "Working hour log not found".BadRequest();
                    
                }

                workingHourLog.TimeOut = DateTime.Now;
                workingHourLog.WorkDuration = workingHourLog.TimeOut - workingHourLog.TimeIn;
                workingHourLog.CreatedById = userId;

                await Repository.Update(workingHourLog);
                await UnitOfWork.SaveAsync();
                var workingHourLogRes = new WorkingHourLogRes
                {
                    EmployeeId = workingHourLog.EmployeeId,
                    Date = workingHourLog.Date,
                    TimeIn = workingHourLog.TimeIn,
                    TimeOut = workingHourLog.TimeOut,
                    WorkDuration = workingHourLog.WorkDuration
                };

                await UnitOfWork.CommitTransactionAsync(transaction);

                return workingHourLogRes.Ok();
            }
            catch (Exception e)
            {
                await UnitOfWork.RollBackTransactionAsync(transaction);
                return e.Message.BadRequest();
            }
        }

        public override async Task<IActionResult> GetAll()
        {
            try
            {
                var workingHourLog = new List<WorkingHourLogRes>();
                var (role, userId) = _contextAccessor.HttpContext.GetRoleAndId();
                var data = await Repository.GetAll();
                foreach (var item in data)
                {
                    workingHourLog.Add(new WorkingHourLogRes
                    {
                        EmployeeId = item.EmployeeId,
                        Date = item.Date,
                        TimeIn = item.TimeIn,
                        TimeOut = item.TimeOut,
                        WorkDuration = item.WorkDuration
                    });
                }

                return workingHourLog.Ok();
            }
            catch (Exception e)
            {
                return e.Message.BadRequest();
            }
        }

        public override async Task<IActionResult> Get(long id)
        {
            try
            {
                var workingHourLog = new WorkingHourLogRes();
                var (role, userId) = _contextAccessor.HttpContext.GetRoleAndId();
                var data = await Repository.Get(id);
                workingHourLog = new WorkingHourLogRes
                {
                    EmployeeId = data.EmployeeId,
                    Date = data.Date,
                    TimeIn = data.TimeIn,
                    TimeOut = data.TimeOut,
                    WorkDuration = data.WorkDuration
                };

                return workingHourLog.Ok();
            }
            catch (Exception e)
            {
                return e.Message.BadRequest();
            }
        }

        public override async Task<IActionResult> Delete(long id)
        {
            try
            {
                var workingHourLog = await Repository.Get(id);
                if (workingHourLog == null)
                {
                    return "Working hour log not found".BadRequest();
                }

                await Repository.Delete(workingHourLog.Id);
                await UnitOfWork.SaveAsync();

                return "Working hour log deleted successfully".Ok();
            }
            catch (Exception e)
            {
                return e.Message.BadRequest();
            }
        }

        public async Task<IActionResult> Update(WorkingHourLogReq reqModel)
        {
            var transaction = await UnitOfWork.BeginTransactionAsync();

            try
            {
                var (role, userId) = _contextAccessor.HttpContext.GetRoleAndId();
                var user = await UnitOfWork.Context.Users.FindAsync(userId);
                if (user == null)
                {
                    return "User not found".BadRequest();
                }

                var employee = await UnitOfWork.Context.Employees.FindAsync(reqModel.EmployeeId);
                if (employee == null)
                {
                    return "Employee not found".BadRequest();
                }
                var workingHourLog = await Repository.Get(reqModel.EmployeeId);
                if (workingHourLog == null)
                {
                    return "Working hour log not found".BadRequest();
                }

                workingHourLog.Employee = employee;
                workingHourLog.Date = reqModel.Date;
                workingHourLog.TimeIn = reqModel.TimeIn;
                workingHourLog.TimeOut = reqModel.TimeOut;
                workingHourLog.WorkDuration = reqModel.WorkDuration;

                await Repository.Update(workingHourLog);
                await UnitOfWork.SaveAsync();
                var workingHourLogRes = new WorkingHourLogRes
                {
                    EmployeeId = workingHourLog.EmployeeId,
                    Date = workingHourLog.Date,
                    TimeIn = workingHourLog.TimeIn,
                    TimeOut = workingHourLog.TimeOut,
                    WorkDuration = workingHourLog.WorkDuration
                };

                await UnitOfWork.CommitTransactionAsync(transaction);

                return workingHourLogRes.Ok();
            }
            catch (Exception e)
            {
                await UnitOfWork.RollBackTransactionAsync(transaction);
                return e.Message.BadRequest();
            }
        }
        



    }
}

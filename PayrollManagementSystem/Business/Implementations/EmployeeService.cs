using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PayrollManagementSystem.Business.Dtos.Request;
using PayrollManagementSystem.Business.Dtos.Response;
using PayrollManagementSystem.Business.Interfaces;
using PayrollManagementSystem.Data.Repositories.Implementations;
using PayrollManagementSystem.Data.UnitOfWork;
using PayrollManagementSystem.Entities;
using TimeWise.Business.Implementations;

namespace PayrollManagementSystem.Business.Implementations
{
    public class EmployeeService : BaseService<EmployeeReq, EmployeeRes, EmployeeRepository, Employee>, IEmployeeService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public EmployeeService(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor
            ) : base(unitOfWork)
        {
            _contextAccessor = contextAccessor;
        }


        public async Task<IActionResult> Add(EmployeeReq reqModel)
        {
            var transaction = await UnitOfWork.BeginTransactionAsync();

            try
            {
                var (role, userId) = _contextAccessor.HttpContext.GetRoleAndId();

                var user = await UnitOfWork.Context.Users.FindAsync(reqModel.AppUserId);
                if (user == null) { 
                    return "User not found".BadRequest();
                }

                var employee = new Employee
                {
                    AppUser = user, 
                    DateOfBirth = reqModel.DateOfBirth,
                    JobTitleId = reqModel.JobTitleId,
                    Address = reqModel.Address,
                    EmploymentStart = reqModel.EmploymentStart,
                    DepartmentId = reqModel.DepartmentId 
                };

                await Repository.Add(employee);
                await UnitOfWork.SaveAsync();
                var employeeRes = new EmployeeRes
                {
                    Id = employee.Id,
                    DateOfBirth = employee.DateOfBirth,
                    JobTitleId = employee.JobTitleId,
                    DepartmentId = employee.DepartmentId,
                    Address = employee.Address,
                    EmploymentStart = employee.EmploymentStart,

                };

                await UnitOfWork.CommitTransactionAsync(transaction);

                return employeeRes.Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await UnitOfWork.RollBackTransactionAsync(transaction);
                return e.HandleError().BadRequest();
            }
        }



        public async Task<IActionResult> GetAll()
        {
            try
            {
                var employee = new List<EmployeeRes>();
                var (role, userId) = _contextAccessor.HttpContext.GetRoleAndId();
                var data = await Repository.GetAll();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        employee.Add(new EmployeeRes
                        {
                            Id = item.Id,
                            DateOfBirth = item.DateOfBirth,
                            JobTitleId = item.JobTitleId,
                            DepartmentId = item.DepartmentId,
                            Address = item.Address,
                            EmploymentStart = item.EmploymentStart,
                        });
                    }
                }
                return employee.Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return e.HandleError().BadRequest();
            }
        }

        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var employee = new EmployeeRes();
                var (role, userId) = _contextAccessor.HttpContext.GetRoleAndId();
                var data = await Repository.Get(id);
                if (data == null) { 
                    return "Employee Not Found".BadRequest(); 
                }
                if (data != null)
                {
                    employee = new EmployeeRes
                    {
                        Id = data.Id,
                        DateOfBirth = data.DateOfBirth,
                        JobTitleId = data.JobTitleId,
                        DepartmentId = data.DepartmentId,
                        Address = data.Address,
                        EmploymentStart = data.EmploymentStart,
                    };
                }
                return employee.Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return e.HandleError().BadRequest();
            }
        }
        public async Task<IActionResult> Update(EmployeeReq reqModel)
        {
            var transaction = await UnitOfWork.BeginTransactionAsync();
            try
            {
                var (role, userId) = _contextAccessor.HttpContext.GetRoleAndId();
                var employee = await Repository.Get(reqModel.Id);
                if (employee == null)
                {
                    return "Employee Not Found".BadRequest();
                }
                employee.DateOfBirth = reqModel.DateOfBirth;
                employee.JobTitleId = reqModel.JobTitleId;
                employee.DepartmentId = reqModel.DepartmentId;
                employee.Address = reqModel.Address;
                employee.EmploymentStart = reqModel.EmploymentStart;

                var ss = await Repository.Update(employee, null);
                await UnitOfWork.SaveAsync();
                await UnitOfWork.CommitTransactionAsync(transaction);
                return ss.Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await UnitOfWork.RollBackTransactionAsync(transaction);
                return e.HandleError().BadRequest();
            }
        }   

        public async Task<IActionResult> Delete(long id)
        {
            var transaction = await UnitOfWork.BeginTransactionAsync();
            try
            {
                var (role, userId) = _contextAccessor.HttpContext.GetRoleAndId();
                var employee = await Repository.Get(id);
                if (employee == null)
                {
                    return "Employee Not Found".BadRequest();
                }
                var res = await Repository.Delete(id);
                await UnitOfWork.SaveAsync();
                await UnitOfWork.CommitTransactionAsync(transaction);
                return res.Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await UnitOfWork.RollBackTransactionAsync(transaction);
                return e.HandleError().BadRequest();
            }
        }
    }
}

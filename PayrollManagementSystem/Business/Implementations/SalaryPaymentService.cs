using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayrollManagementSystem.Business.Dtos.Request;
using PayrollManagementSystem.Business.Dtos.Response;
using PayrollManagementSystem.Business.Interfaces;
using PayrollManagementSystem.Data.Repositories.Implementations;
using PayrollManagementSystem.Data.UnitOfWork;
using System.Linq;
using TimeWise.Business.Implementations;

namespace PayrollManagementSystem.Business.Implementations
{
    public class SalaryPaymentService : BaseService<SalaryPaymentReq, SalaryPaymentRes, SalaryPaymentRepository, SalaryPayment>, ISalaryPaymentService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public SalaryPaymentService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor) : base(unitOfWork)
        {
            _contextAccessor = contextAccessor;
        }

        public override async Task<IActionResult> Add (SalaryPaymentReq salaryPaymentReq)
        {
            var transaction = await UnitOfWork.BeginTransactionAsync();
            try {
                var (role, userId) = _contextAccessor.HttpContext.GetRoleAndId();
                var user = await UnitOfWork.Context.Users.FindAsync(userId);
                if (user == null)
                {
                    return "User not found".BadRequest();
                }
                var employee = await UnitOfWork.Context.Employees.FindAsync(salaryPaymentReq.EmployeeId);
                if (employee == null)
                {
                    return "Employee not found".BadRequest();
                }
                var salaryPayment = new SalaryPayment
                {
                    Employee = employee,
                    GrossSalary = salaryPaymentReq.GrossSalary,
                    NetSalary = salaryPaymentReq.NetSalary,
                    SalaryPeriod = salaryPaymentReq.SalaryPeriod
                };

                await Repository.Add(salaryPayment);
                await UnitOfWork.SaveAsync();
                var salaryPaymentRes = new SalaryPaymentRes
                {
                    EmployeeId = salaryPayment.EmployeeId,
                    GrossSalary = salaryPayment.GrossSalary,
                    NetSalary = salaryPayment.NetSalary,
                    SalaryPeriod = salaryPayment.SalaryPeriod
                };
                await UnitOfWork.CommitTransactionAsync(transaction);
                return salaryPaymentRes.Ok();

            }
            catch(Exception e)
            {
                await UnitOfWork.RollBackTransactionAsync(transaction);
                return e.Message.BadRequest();
            }
           
        }

        public override async Task<IActionResult> Update(SalaryPaymentReq salaryPaymentReq)
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
                var employee = await UnitOfWork.Context.Employees.FindAsync(salaryPaymentReq.EmployeeId);
                if (employee == null)
                {
                    return "Employee not found".BadRequest();
                }
                var salaryPayment = await UnitOfWork.Context.SalaryPayments.FindAsync(salaryPaymentReq.EmployeeId);
                if (salaryPayment == null)
                {
                    return "Salary Payment not found".BadRequest();
                }
                salaryPayment.Employee = employee;
                salaryPayment.GrossSalary = salaryPaymentReq.GrossSalary;
                salaryPayment.NetSalary = salaryPaymentReq.NetSalary;
                salaryPayment.SalaryPeriod = salaryPaymentReq.SalaryPeriod;

                await Repository.Update(salaryPayment);
                await UnitOfWork.SaveAsync();
                var salaryPaymentRes = new SalaryPaymentRes
                {
                    EmployeeId = salaryPayment.EmployeeId,
                    GrossSalary = salaryPayment.GrossSalary,
                    NetSalary = salaryPayment.NetSalary,
                    SalaryPeriod = salaryPayment.SalaryPeriod
                };
                await UnitOfWork.CommitTransactionAsync(transaction);
                return salaryPaymentRes.Ok();

            }
            catch (Exception e)
            {
                await UnitOfWork.RollBackTransactionAsync(transaction);
                return e.Message.BadRequest();
            }
        }

        public override async Task<IActionResult> Delete(long id)
        {
            var transaction = await UnitOfWork.BeginTransactionAsync();
            try
            {
                var salaryPayment = await UnitOfWork.Context.SalaryPayments.FindAsync(id);
                if (salaryPayment == null)
                {
                    return "Salary Payment not found".BadRequest();
                }
                await Repository.Delete(salaryPayment.Id);
                await UnitOfWork.SaveAsync();
                await UnitOfWork.CommitTransactionAsync(transaction);
                return "Salary Payment deleted successfully".Ok();
            }
            catch (Exception e)
            {
                await UnitOfWork.RollBackTransactionAsync(transaction);
                return e.Message.BadRequest();
            }
        }

        public override async Task<IActionResult> Get(long id)
        {
            try
            {
                var salaryPayment = await UnitOfWork.Context.SalaryPayments.FindAsync(id);
                if (salaryPayment == null)
                {
                    return "Salary Payment not found".BadRequest();
                }
                var salaryPaymentRes = new SalaryPaymentRes
                {
                    EmployeeId = salaryPayment.EmployeeId,
                    GrossSalary = salaryPayment.GrossSalary,
                    NetSalary = salaryPayment.NetSalary,
                    SalaryPeriod = salaryPayment.SalaryPeriod
                };
                return salaryPaymentRes.Ok();
            }
            catch (Exception e)
            {
                return e.Message.BadRequest();
            }
        }


    }

}

using Microsoft.AspNetCore.Mvc;
using PayrollManagementSystem.Business.Dtos.Request;
using PayrollManagementSystem.Business.Dtos.Response;
using PayrollManagementSystem.Business.Interfaces;
using PayrollManagementSystem.Data.Repositories.Implementations;
using PayrollManagementSystem.Data.UnitOfWork;
using TimeWise.Business.Implementations;

namespace PayrollManagementSystem.Business.Implementations
{
    public class EmploymentTermService : BaseService<EmploymentTermReq, EmploymentTermRes, EmploymentTermRepository, EmploymentTerm>, IEmploymentTermService
    {
       private readonly IHttpContextAccessor _contextAccessor;
        public EmploymentTermService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor) : base(unitOfWork)
        {
            _contextAccessor = contextAccessor;
        }

        public override async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await Repository.GetAll();
                var res = new List<EmploymentTermRes>();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        res.Add(new EmploymentTermRes
                        {
                            Id = item.Id,
                            EmployeeId = item.EmployeeId,
                            SalaryStartDate = item.SalaryStartDate,
                            SalaryEndDate = item.SalaryEndDate,
                            AgreedSalary = item.AgreedSalary,
                       
                        });
                    }
                }
                return res.Ok();
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return e.HandleError().BadRequest();
            }
        }

        public override async Task<IActionResult> Get(long id)
        {
            try
            {
                var data = await Repository.Get(id);
                var res = new EmploymentTermRes
                {
                    Id = data.Id,
                    EmployeeId = data.EmployeeId,
                    SalaryStartDate = data.SalaryStartDate,
                    SalaryEndDate = data.SalaryEndDate,
                    AgreedSalary = data.AgreedSalary,
                };
                return res.Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return e.HandleError().BadRequest();
            }
        }

        public override async Task<IActionResult> Add(EmploymentTermReq reqModel)
        {
            var transaction = await UnitOfWork.BeginTransactionAsync();
            try
            {
                var (role, userId) = _contextAccessor.HttpContext.GetRoleAndId();
                var employee = UnitOfWork.Context.Employees.FirstOrDefault(x => x.Id == reqModel.EmployeeId);
                if (employee == null)
                {
                       return new Exception("Employee not found").HandleError().BadRequest();
                }
                var data = await Repository.Add(new EmploymentTerm
                {
                    EmployeeId = reqModel.EmployeeId,
                    SalaryStartDate = reqModel.SalaryStartDate,
                    SalaryEndDate = reqModel.SalaryEndDate,
                    AgreedSalary = reqModel.AgreedSalary,
                });
                await UnitOfWork.Context.SaveChangesAsync();
                await transaction.CommitAsync();
                return data.Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await transaction.RollbackAsync();
                return e.HandleError().BadRequest();
            }
        }

        public override async Task<IActionResult> Update(EmploymentTermReq reqModel)
        {
            var transaction = await UnitOfWork.BeginTransactionAsync();
            try
            {
                var data = await Repository.Update(new EmploymentTerm
                {
                    Id = reqModel.Id,
                    EmployeeId = reqModel.EmployeeId,
                    SalaryStartDate = reqModel.SalaryStartDate,
                    SalaryEndDate = reqModel.SalaryEndDate,
                    AgreedSalary = reqModel.AgreedSalary,
                }, null);
                await UnitOfWork.Context.SaveChangesAsync();
                await transaction.CommitAsync();
                return data.Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await transaction.RollbackAsync();
                return e.HandleError().BadRequest();
            }
        }

        public override async Task<IActionResult> Delete(long id)
        {
            var transaction = await UnitOfWork.BeginTransactionAsync();
            try
            {
                var data = await Repository.Delete(id);
                await UnitOfWork.Context.SaveChangesAsync();
                await transaction.CommitAsync();
                return data.Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await transaction.RollbackAsync();
                return e.HandleError().BadRequest();
            }
        }

    }
}

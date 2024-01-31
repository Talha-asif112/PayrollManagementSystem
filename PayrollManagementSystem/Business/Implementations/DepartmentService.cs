using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayrollManagementSystem.Business.Dtos.Request;
using PayrollManagementSystem.Business.Dtos.Response;
using PayrollManagementSystem.Business.Interfaces;
using PayrollManagementSystem.Data.Repositories.Implementations;
using PayrollManagementSystem.Data.UnitOfWork;
using TimeWise.Business.Implementations;
using TimeWise.Domain.Interfaces;

namespace PayrollManagementSystem.Business.Implementations
{
    public class DepartmentService : BaseService<DepartmentReq, DepartmentRes, DepartmentRepository, Department>, IDepartmentService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public DepartmentService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor) : base(unitOfWork)
        {
            _contextAccessor = contextAccessor;
        }

        public override async Task<IActionResult> GetAll()
        {
            try
            {
                var department = new List<DepartmentRes>();
                var (role, userId) = _contextAccessor.HttpContext.GetRoleAndId();
                var data = await Repository.GetAll();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        department.Add(new DepartmentRes
                        {
                            Id = item.Id,
                            DepartmentName = item.DepartmentName
                        });
                    }
                }
                return department.Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return e.HandleError().BadRequest();
            }

        }

        public override async Task<IActionResult> Get(long id)
        {
            try
            {
                var department = new DepartmentRes();
                var (role, userId) = _contextAccessor.HttpContext.GetRoleAndId();
                var data = await Repository.Get(id);
                if (data != null)
                {
                    department.Id = data.Id;
                    department.DepartmentName = data.DepartmentName;
                }
                return department.Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return e.HandleError().BadRequest();
            }
        }

        public override async Task<IActionResult> Add(DepartmentReq reqModel)
        {
            var transaction = await UnitOfWork.BeginTransactionAsync();
            try
            {
                var (role, userId) = _contextAccessor.HttpContext.GetRoleAndId();

                Department department = new Department
                {
                    DepartmentName = reqModel.DepartmentName,
                    CreatedDate = DateTime.UtcNow,
                    CreatedById = userId,
                    IsActive = true
                };

                var existingDepartment = await UnitOfWork.Context.Departments.FirstOrDefaultAsync(c => c.DepartmentName == reqModel.DepartmentName && c.IsDelete != true);
                if (existingDepartment != null)
                {
                    return ("Department Already exist with the same Name.").BadRequest();
                }

                var ss = await Repository.Add(department);
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

        public override async Task<IActionResult> Update(DepartmentReq reqModel)
        {
            var transaction = await UnitOfWork.BeginTransactionAsync();
            try
            {
                var (role, userId) = _contextAccessor.HttpContext.GetRoleAndId();
                var res = await Repository.Update((Department)(reqModel as IMinBase ??
                 throw new InvalidOperationException( 
                     "Conversion to IMinBase Failed. Make sure there's Id and CreatedDate properties.")));

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

        public override async Task<IActionResult> Delete(long id)
        {
            var transaction = await UnitOfWork.BeginTransactionAsync();
            try
            {
                var (role, userId) = _contextAccessor.HttpContext.GetRoleAndId();
                var res = await Repository.Update((Department)(new DepartmentReq() as IMinBase ??
                                    throw new InvalidOperationException(
                                                            "Conversion to IMinBase Failed. Make sure there's Id and CreatedDate properties.")));

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

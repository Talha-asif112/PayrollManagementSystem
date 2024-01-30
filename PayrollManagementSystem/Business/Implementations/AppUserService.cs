using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PayrollManagementSystem.Business.Dtos.Request;
using PayrollManagementSystem.Business.Dtos.Response;
using PayrollManagementSystem.Business.Interfaces;
using PayrollManagementSystem.Data;
using PayrollManagementSystem.Data.Repositories.Implementations;
using PayrollManagementSystem.Data.UnitOfWork;
using PayrollManagementSystem.Entities;
using System.IdentityModel.Tokens.Jwt;
using TimeWise.Business.Implementations;

namespace PayrollManagementSystem.Business.Implementations
{
    public class AppUserService : BaseService<AppUserReq, AppUserRes, AppUserRepository, AppUser>, IAppUserService
    {

        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AppUserService(
            IUnitOfWork unitOfWork,
            IConfiguration configuration,
            IHttpContextAccessor contextAccessor,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager

            ) : base(unitOfWork)
        {
            _configuration = configuration;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _signInManager = signInManager;
        }

    
        public override async Task<IActionResult> Get(long id)
        {
            try
            {
                var appUser = new AppUserRes();
                var (role, userId) = _contextAccessor.HttpContext.GetRoleAndId();
                var data = await Repository.Get(id);
                if (data != null)
                {
                    appUser.Id = data.Id;
                    appUser.Email = data.Email;
                    appUser.UserName = data.UserName;
                    appUser.FirstName = data.FirstName;
                    appUser.LastName = data.LastName;
                    appUser.ContactNo = data.ContactNo;
                    appUser.Role = data.Role;
                }
                return appUser.Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return e.HandleError().BadRequest();
            }
        }

        public override async Task<IActionResult> GetAll()
        {
            try
            {
                var appUser = new List<AppUserRes>();
                var (role, userId) = _contextAccessor.HttpContext.GetRoleAndId();
                var data = await Repository.GetAll();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        appUser.Add(new AppUserRes
                        {
                            Id = item.Id,
                            Email = item.Email,
                            UserName = item.UserName,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            ContactNo = item.ContactNo,
                            Role = item.Role,
                        });
                    }
                }
                return appUser.Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return e.HandleError().BadRequest();
            }
        }
        public override async Task<IActionResult> Add(AppUserReq reqModel)
        {
            var transaction = await UnitOfWork.BeginTransactionAsync();
            try
            {
                var userId = _contextAccessor.HttpContext.GetUserId();
                AppUser user = new AppUser
                {
                    Id = userId,
                    Email = reqModel.Email,
                    UserName = reqModel.UserName,
                    FirstName = reqModel.FirstName,
                    LastName = reqModel.LastName,
                    ContactNo = reqModel.ContactNo != null ? reqModel.ContactNo : null,
                    Role = reqModel.Role,
                    CreatedDate = DateTime.UtcNow,
                    CreatedById = userId,

                };

                var existinguser = await UnitOfWork.Context.AppUsers.FirstOrDefaultAsync(c => c.Email == reqModel.Email && c.IsDelete != true);
                if (existinguser != null)
                {
                    return ("User Already exist with the same Email.").BadRequest();
                }

                var result = await _signInManager.UserManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return string.Join(", ", result.Errors.Select(f => f.Description)).BadRequest();
                }
                await UnitOfWork.SaveAsync();
                await UnitOfWork.CommitTransactionAsync(transaction);
                AppUserRes res = new AppUserRes
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ContactNo = user.ContactNo,
                    Role = user.Role,
                };
                return res.Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return e.HandleError();
            }
        }
     
        public async Task<IActionResult> LoginUser(LoginReq req)
        {
            try
            {
                AppUser? appUser = null;

                appUser = UnitOfWork.Context.Users.FirstOrDefault(x => x.UserName == req.UserName);

                    if (appUser == null)
                    {
                        return "".NotFound("No Account found with this username.");
                    }

                    //var ss = await _signInManager.CheckPasswordSignInAsync(appUser, req.Password, false);
                    var token = appUser.GenerateJwt(_configuration);
                   /* if (!ss.Succeeded)
                        return (ss.IsLockedOut
                            ? "You can't login. You account has been locked."
                            : ss.IsNotAllowed
                                ? "User is not allowed to log in. Contact Support."
                                : "Incorrect User Password Entered!!").BadRequest();*/
                    var userData = new
                    {
                        Name = $"{appUser.FirstName} {appUser.LastName}",
                        appUser.Role,
                        appUser.UserName,
                        appUser.Email,
                        UserId = appUser.Id,
                    };

                    // var final = new { AccessToken = token, UserData = userData };
                    return userData.Ok("User Login Successful!", new { AccessToken = token });
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}

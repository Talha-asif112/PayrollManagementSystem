using HisaabRakh.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using PayrollManagementSystem.Business;
using PayrollManagementSystem.Business.Implementations;
using PayrollManagementSystem.Business.Interfaces;
using PayrollManagementSystem.Context;
using PayrollManagementSystem.Data.UnitOfWork;
using PayrollManagementSystem.Entities;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region For Services Injection

builder.Services.AddScoped<IAdjustmentService, AdjustmentService>();
builder.Services.AddScoped<IAdjustmentAmountService, AdjustmentAmountService>();
builder.Services.AddScoped<IAppUserService, AppUserService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmploymentTermService, EmploymentTermService>();
builder.Services.AddScoped<IJobTitleService, JobTitleService>();
builder.Services.AddScoped<ISalaryPaymentService, SalaryPaymentService>();
builder.Services.AddScoped<IWorkHourAdjustmentService, WorkHourAdjustmentService>();
builder.Services.AddScoped<IWorkingHourLogService, WorkingHourLogService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.UseAllOfToExtendReferenceSchemas();
    options.OperationFilter<AuthResponsesOperationFilter>();
});

builder.Services.AddDbContext<PayrollDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("PayrollDb")); 
});


builder.Services.AddIdentity<AppUser, IdentityRole<long>>(opt =>
{
    opt.User.RequireUniqueEmail = true;
    opt.SignIn.RequireConfirmedEmail = true;
    opt.Tokens.EmailConfirmationTokenProvider = builder.Configuration["TokenProviders:email"]!;
    opt.Tokens.PasswordResetTokenProvider = builder.Configuration["TokenProviders:resetPassword"]!;
})
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<PayrollDbContext>();


#region For JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = builder.Configuration.GetValidationParameters();
    });

#endregion


#region For Authorization
builder.Services.AddAuthorization(options =>
{
    var policyBuilder = new AuthorizationPolicyBuilder();
    var defaultPolicy = policyBuilder
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
    options.DefaultPolicy = defaultPolicy;
});
#endregion
var app = builder.Build();

app.UseCors(policyBuilder =>
{
    policyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
        .WithExposedHeaders("token", "www-authenticate");
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

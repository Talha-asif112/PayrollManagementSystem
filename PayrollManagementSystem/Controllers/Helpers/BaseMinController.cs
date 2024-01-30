/*using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeWise.Business.Interfaces;

namespace TimeWise.API.Helpers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class BaseMinController<T, TService> : ControllerBase where TService : IBaseMinService
{
    protected readonly TService Service;
    protected readonly ILogger<T> Logger;

    public BaseMinController(ILogger<T> logger, TService service)
    {
        Service = service;
        Logger = logger;
    }

    ~BaseMinController()
    {
        Logger.LogInformation("Instance Destroyed!");
    }
}*/
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeWise.Business.Interfaces;
using Utilities;

namespace TimeWise.API.Helpers;

/// <summary>
/// BaseController
/// </summary>
/// <typeparam name="T">The Controller Reference</typeparam>
/// <typeparam name="TService">The Service Interface</typeparam>
/// <typeparam name="TReq">The Request Object</typeparam>
/// <typeparam name="TRes">The Response Object</typeparam>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class BaseController<T, TService, TReq, TRes> : ControllerBase where TService : IBaseService<TReq, TRes>
{
    private readonly ILogger<T> _logger;
    protected readonly TService Service;

    public BaseController(ILogger<T> logger, TService service)
    {
        _logger = logger;
        Service = service;
    }


    /// <summary>
    /// This Return a List of Addresses
    /// </summary>
    /// <see cref="GetAttribute"/>
    /// <returns></returns>
    /// <remarks>Remember this is Only For Specific Reasons.
    /// If you want to do something Else, ignore this</remarks>
    /// <exception cref="NotFoundResult">When Result isn't Found!</exception>
    /// <seealso cref="ApplicationIdentity"/>
    /// <response code="200">Data Added!</response>
    [HttpGet]
    [ProducesResponseType(typeof(IList<Response<object>>), 200)]
    [ProducesDefaultResponseType(typeof(string))]
    [ProducesResponseType(typeof(bool), 201)]
    [ProducesResponseType(typeof(string), 404)]
    public virtual async Task<IActionResult> Get()
    {
        return await Service.GetAll();
    }


    /// <summary>
    /// This Returns with Id
    /// </summary>
    /// <param name="id">Id in Long. like: 80,12</param>
    /// <returns>A Good List Of Something?</returns>
    /// 
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(bool), 200)]
    public virtual async Task<IActionResult> Get(long id)
    {
        return await Service.Get(id);
    }

    [HttpPost]
    public virtual async Task<IActionResult> Post(TReq model)
    {
        return await Service.Add(model);
    }

    [HttpPut]
    public virtual async Task<IActionResult> Put(TReq model)
    {
        return await Service.Update(model);
    }

    [HttpDelete("{id:long}")]
    public virtual async Task<IActionResult> Delete(long id)
    {
        return await Service.Delete(id);
    }


    ~BaseController()
    {
        _logger.LogInformation("Instance Destroyed!");
    }

    public static string GetVersionedBase(string action)
    {
        return "api/v{version:apiVersion}/" + action;
    }
}
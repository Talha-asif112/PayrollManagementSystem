using Microsoft.AspNetCore.Mvc;

namespace TimeWise.Business.Interfaces;

public interface IBaseService<in TDto, out TResp>
{
    public Task<IActionResult> GetAll();
    public Task<IActionResult> Get(long id);
    public Task<IActionResult> Add(TDto model);
    public Task<IActionResult> Update(TDto model);
    public Task<IActionResult> Delete(long id);
}
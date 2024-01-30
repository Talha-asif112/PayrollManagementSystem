
public interface IBaseRepository<TEntity>
{

    public Task<IList<TEntity>> GetAll();

    /// <summary>
    /// Get the Entity with Id
    /// </summary>
    /// <param name="id">Valid id for the entity</param>
    /// <returns></returns>
    public Task<TEntity> Get(long id);

    /// <summary>
    /// Add the Entity to DB
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task<TEntity> Add(TEntity model);

    /// <summary>
    /// Update the Db Entity. 
    /// </summary>
    /// <param name="model">The Source Model</param>
    /// <param name="func">The Update Function for Mapping Entity Properties</param>
    /// <returns></returns>
    public Task<TEntity> Update(TEntity model, Func<TEntity, TEntity>? func);

    /// <summary>
    /// Delete an Entity with the Specified Id
    /// </summary>
    /// <param name="id">Valid Id of the Entity</param>
    /// <returns></returns>
    public Task<bool> Delete(long id);
}
using ETASY_OMS_PROJECT.WebUI.Entity.Entities.Abstract;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities.Concrete;

namespace ETASY_OMS_PROJECT.WebUI.DAL.Repos.Abstract
{
    public interface IGenericRepository<TEntity> where TEntity : EntityBase
    {
        TEntity Get(int id);
        Task<List<TEntity>> GetAllAsync();
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(int id);
        Task SaveAsync();

    }
}

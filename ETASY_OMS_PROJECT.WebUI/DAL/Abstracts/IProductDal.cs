using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Abstract;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;

namespace ETASY_OMS_PROJECT.WebUI.DAL.Abstracts
{
    public interface IProductDal : IGenericRepository<Product>
    {
        Task<bool> CheckCodeAsync(string code);
        Task<bool> CheckCodeAsync(Guid id, string code);
        Task<bool> CheckNameAsync(string name);
        Task<bool> CheckNameAsync(Guid id, string name);
    }
}

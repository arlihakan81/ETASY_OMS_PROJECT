using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Abstract;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;

namespace ETASY_OMS_PROJECT.WebUI.DAL.Abstracts
{
    public interface ISupplierDal : IGenericRepository<Supplier>
    {
        Task<bool> CheckSupplierCodeAsync(string code);
        Task<bool> CheckSupplierCodeAsync(Guid id, string code);

        Task<bool> CheckSupplierNameAsync(string name);
        Task<bool> CheckSupplierNameAsync(Guid id, string name);

    }
}

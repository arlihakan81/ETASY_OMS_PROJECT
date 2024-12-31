using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Abstract;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;

namespace ETASY_OMS_PROJECT.WebUI.DAL.Abstracts
{
    public interface ICustomerDal : IGenericRepository<Customer>
    {
        Task<bool> CheckNameAsync(string name);
        Task<bool> CheckNameAsync(int id, string name);
    }
}

using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Abstract;

namespace ETASY_OMS_PROJECT.WebUI.DAL.Abstracts
{
    public interface IGroupDal : IGenericRepository<Group>
    {
        Task<bool> CheckGroupCodeAsync(string code);
        Task<bool> CheckGroupCodeAsync(Guid id, string code);
    }
}

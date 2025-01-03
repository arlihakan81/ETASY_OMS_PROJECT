using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Abstract;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;

namespace ETASY_OMS_PROJECT.WebUI.DAL.Abstracts
{
    public interface INotificationDal : IGenericRepository<Notification>
    {
        Task<List<Notification>> GetAllAsync(int id);
    }
}

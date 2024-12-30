using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Abstract;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using ETASY_OMS_PROJECT.WebUI.Entity.ViewModels;

namespace ETASY_OMS_PROJECT.WebUI.DAL.Abstracts
{
    public interface IAccountDal : IGenericRepository<User>
    {
        Task<bool> CheckUsernameAsync(string Username);
        Task<bool> CheckUsernameAsync(int id, string Username);
        Task<User> CheckAccountAsync(LoginViewModel model);
        Task<bool> CheckResetAsync(int id, ResetViewModel model);
    }
}

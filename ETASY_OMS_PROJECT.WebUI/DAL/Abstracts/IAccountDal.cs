using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Abstract;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using ETASY_OMS_PROJECT.WebUI.Entity.ViewModels;
using ETASY_OMS_PROJECT.WebUI.Entity.ViewModels.AccountVM;

namespace ETASY_OMS_PROJECT.WebUI.DAL.Abstracts
{
    public interface IAccountDal : IGenericRepository<User>
    {
        Task<bool> CheckUsernameAsync(string Username);
        Task<bool> CheckUsernameAsync(Guid id, string Username);
        Task<User> CheckAccountAsync(LoginViewModel model);
        Task<bool> CheckResetAsync(Guid id, ResetViewModel model);
        CreateAccountModel GetCreateAccountModel();
        UpdateAccountModel GetUpdateAccountModel(Guid id);
    }
}

using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Abstract;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;

namespace ETASY_OMS_PROJECT.WebUI.DAL.Abstracts
{
    public interface IMaterialDal : IGenericRepository<Material>
    {
        Task<bool> CheckMaterialCodeAsync(string code);
        Task<bool> CheckMaterialCodeAsync(Guid id, string code);

        Task<bool> CheckMaterialNameAsync(string name);
        Task<bool> CheckMaterialNameAsync(Guid id, string name);
    }
}

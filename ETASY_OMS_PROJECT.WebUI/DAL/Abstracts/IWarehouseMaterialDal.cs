using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Abstract;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using ETASY_OMS_PROJECT.WebUI.Entity.ViewModels.WarehouseMaterialVM;

namespace ETASY_OMS_PROJECT.WebUI.DAL.Abstracts
{
    public interface IWarehouseMaterialDal : IGenericRepository<WarehouseMaterial>
    {
        CreateWarehouseMaterialModel GetCreateWarehouseMaterialModel(Guid id); // Warehouse.Id
        UpdateWarehouseMaterialModel GetUpdateWarehouseMaterialModel(Guid id); // WarehouseMaterial.Id
    }
}

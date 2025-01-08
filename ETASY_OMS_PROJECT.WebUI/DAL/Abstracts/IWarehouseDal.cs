using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Abstract;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using ETASY_OMS_PROJECT.WebUI.Entity.ViewModels.WarehouseVM;

namespace ETASY_OMS_PROJECT.WebUI.DAL.Abstracts
{
    public interface IWarehouseDal : IGenericRepository<Warehouse>
    {
        CreateWarehouseModel GetCreateWarehouseModel();
        UpdateWarehouseModel GetUpdateWarehouseModel(Guid id);

    }
}

using ETASY_OMS_PROJECT.WebUI.Entity.Entities;

namespace ETASY_OMS_PROJECT.WebUI.Entity.ViewModels.WarehouseVM
{
    public class CreateWarehouseModel
    {
        public List<Supplier> Suppliers { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}

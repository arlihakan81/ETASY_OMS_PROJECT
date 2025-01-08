using ETASY_OMS_PROJECT.WebUI.Entity.Entities;

namespace ETASY_OMS_PROJECT.WebUI.Entity.ViewModels.WarehouseMaterialVM
{
    public class CreateWarehouseMaterialModel
    {
        public List<Material> Materials { get; set; }
        public List<WarehouseMaterial> WarehouseMaterials { get; set; }
        public WarehouseMaterial WarehouseMaterial { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}

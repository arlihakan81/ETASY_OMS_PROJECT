using ETASY_OMS_PROJECT.WebUI.Entity.Entities.Concrete;

namespace ETASY_OMS_PROJECT.WebUI.Entity.Entities
{
    public class WarehouseMaterial : EntityBase
    {
        public Guid WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
        public Guid MaterialId { get; set; }
        public Material Material { get; set; }
        public int Quantity { get; set; }
    }
}

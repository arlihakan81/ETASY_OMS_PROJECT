using ETASY_OMS_PROJECT.WebUI.Entity.Entities.Concrete;

namespace ETASY_OMS_PROJECT.WebUI.Entity.Entities
{
    public class Warehouse : EntityBase
    {
        public string Code { get; set; }
        public Guid SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        public string Description { get; set; } 



    }
}

using ETASY_OMS_PROJECT.WebUI.Entity.Entities.Abstract;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities.Concrete;

namespace ETASY_OMS_PROJECT.WebUI.Entity.Entities
{
    public class Product : EntityBase
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Scale { get; set; }
    }
}

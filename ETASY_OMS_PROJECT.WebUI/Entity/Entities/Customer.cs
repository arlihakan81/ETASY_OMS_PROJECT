using ETASY_OMS_PROJECT.WebUI.Entity.Entities.Abstract;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities.Concrete;

namespace ETASY_OMS_PROJECT.WebUI.Entity.Entities
{
    public class Customer : EntityBase
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}

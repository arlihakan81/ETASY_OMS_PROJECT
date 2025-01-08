using ETASY_OMS_PROJECT.WebUI.Entity.Entities.Concrete;

namespace ETASY_OMS_PROJECT.WebUI.Entity.Entities
{
    public class Material : EntityBase
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

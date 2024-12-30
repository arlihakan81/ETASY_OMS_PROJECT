using ETASY_OMS_PROJECT.WebUI.Entity.Entities.Abstract;

namespace ETASY_OMS_PROJECT.WebUI.Entity.Entities.Concrete
{
    public class EntityBase : IEntityBase
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

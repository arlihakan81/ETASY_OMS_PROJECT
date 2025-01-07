namespace ETASY_OMS_PROJECT.WebUI.Entity.Entities.Abstract
{
    public interface IEntityBase
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

namespace ETASY_OMS_PROJECT.WebUI.Entity.Entities.Abstract
{
    public interface IEntityBase
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

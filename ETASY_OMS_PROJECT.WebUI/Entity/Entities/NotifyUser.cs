using ETASY_OMS_PROJECT.WebUI.Entity.Entities.Concrete;

namespace ETASY_OMS_PROJECT.WebUI.Entity.Entities
{
    public class NotifyUser : EntityBase
    {
        public Guid UserId { get; set; }
        public Guid NotificationId { get; set; }
        public bool IsRead { get; set; }
        public User User { get; set; }
        public Notification Notification { get; set; }
    }
}

using ETASY_OMS_PROJECT.WebUI.Entity.Entities.Concrete;

namespace ETASY_OMS_PROJECT.WebUI.Entity.Entities
{
    public class NotifyUser : EntityBase
    {
        public int UserId { get; set; }
        public int NotificationId { get; set; }
        public bool IsRead { get; set; }
        public User User { get; set; }
        public Notification Notification { get; set; }
    }
}

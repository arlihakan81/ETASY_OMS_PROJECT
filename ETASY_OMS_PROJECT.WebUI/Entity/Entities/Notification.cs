using ETASY_OMS_PROJECT.WebUI.Entity.Entities.Concrete;
using ETASY_OMS_PROJECT.WebUI.Entity.Enums.Notifications;

namespace ETASY_OMS_PROJECT.WebUI.Entity.Entities
{
    public class Notification : EntityBase
    {
        public Operation Operation { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        
        public List<NotifyUser> NotifyUsers { get; set; }
    }
}

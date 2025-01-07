using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.DAL.Concretes.Data;
using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Concrete;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETASY_OMS_PROJECT.WebUI.DAL.Concretes
{
    public class NotificationDal : GenericRepository<Notification>, INotificationDal
    {
        private readonly ApplicationDbContext _context;
        public NotificationDal(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Notification>> GetAllAsync(Guid id)
        {
            return await _context.Notifications.Include(_ => _.User).ToListAsync();
        }
    }
}

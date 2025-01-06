using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.DAL.Concretes.Data;
using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Concrete;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETASY_OMS_PROJECT.WebUI.DAL.Concretes
{
    public class NotifyUserDal : GenericRepository<NotifyUser>, INotifyUserDal
    {
        private readonly ApplicationDbContext _context;
        public NotifyUserDal(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }

        public override async Task<List<NotifyUser>> GetAllAsync()
        {
            return await _context.NotifyUsers.Include(_ => _.User).Include(_ => _.Notification)
                .ThenInclude(_ => _.User).ToListAsync();
        }

        public override NotifyUser Get(int id)
        {
            return _context.NotifyUsers.Include(_ => _.User).Include(_ => _.Notification)
                .FirstOrDefault(_ => _.Id == id);
        }

    }
}

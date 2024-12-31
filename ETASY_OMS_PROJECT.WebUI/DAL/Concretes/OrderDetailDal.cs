using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.DAL.Concretes.Data;
using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Concrete;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETASY_OMS_PROJECT.WebUI.DAL.Concretes
{
    public class OrderDetailDal : GenericRepository<OrderDetail>, IOrderDetailDal
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailDal(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override OrderDetail Get(int id)
        {
            return _context.OrderDetails.Include(_ => _.Product)
                .FirstOrDefault(_ => _.Id == id);
        }

        public override Task<List<OrderDetail>> GetAllAsync()
        {
            return _context.OrderDetails.Include(_ => _.Product).ToListAsync();
        }

    }
}

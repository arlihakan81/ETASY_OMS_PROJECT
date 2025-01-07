using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.DAL.Concretes.Data;
using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Concrete;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using ETASY_OMS_PROJECT.WebUI.Entity.ViewModels.OrderDetailVM;
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

        public override OrderDetail Get(Guid id) // OrderDetail.Id
        {
            return _context.OrderDetails.Include(_ => _.Product)
                .Include(_ => _.Order).ThenInclude(_ => _.Customer)
                .FirstOrDefault(_ => _.Id == id);
        }

        public override Task<List<OrderDetail>> GetAllAsync()
        {
            return _context.OrderDetails.Include(_ => _.Order).ThenInclude(_ => _.Customer)
                .Include(_ => _.Product).ToListAsync();
        }

        public CreateOrderDetailModel GetCreateOrderDetailModel(Guid id) // Order.Id
        {
            return new CreateOrderDetailModel
            {
                Order = _context.Orders.Include(_ => _.OrderDetail).ThenInclude(_ => _.Product).Include(_ => _.Customer)
                    .Include(_ => _.Department).FirstOrDefault(_ => _.Id == id),
                Products = _context.Products.ToList(),
                OrderDetails = _context.OrderDetails.Include(_ => _.Product)
                    .Include(_ => _.Order).ThenInclude(_ => _.Customer).Where(_ => _.OrderId == id).ToList(),
                OrderDetail = new()
            };
        }

        public DetailOrderDetailModel GetDetailOrderDetailModel(Guid id) // Order.Id
        {
            return new DetailOrderDetailModel
            {
                OrderDetails = _context.OrderDetails.Include(_ => _.Product).Include(_ => _.Order)
                .ThenInclude(_ => _.Customer).Where(_ => _.OrderId == id).ToList(),
                Order = _context.Orders.Include(_ => _.Customer).Include(_ => _.Department)
                .Where(_ => _.Id == id).FirstOrDefault()
            };
        }

        public UpdateOrderDetailModel GetUpdateOrderDetailModel(Guid id) // OrderDetail.Id
        {
            return new UpdateOrderDetailModel
            {
                Products = _context.Products.ToList(),
                Order = _context.Orders.Include(_ => _.OrderDetail).ThenInclude(_ => _.Product).Include(_ => _.Customer)
                    .Include(_ => _.Department).FirstOrDefault(_ => _.OrderDetail.Id == id),
                OrderDetails = _context.OrderDetails.Include(_ => _.Product)
                    .Include(_ => _.Order).ThenInclude(_ => _.Customer).Where(_ => _.Id == id).ToList(),
                OrderDetail = _context.OrderDetails.Include(_ => _.Product)
                    .Include(_ => _.Order).ThenInclude(_ => _.Customer).FirstOrDefault(_ => _.Id == id)
            };
        }
    }
}

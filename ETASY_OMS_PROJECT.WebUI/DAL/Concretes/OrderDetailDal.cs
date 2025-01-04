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

        public override OrderDetail Get(int id)
        {
            return _context.OrderDetails.Include(_ => _.Product)
                .Include(_ => _.Order)
                .FirstOrDefault(_ => _.Id == id);
        }

        public override Task<List<OrderDetail>> GetAllAsync()
        {
            return _context.OrderDetails.Include(_ => _.Order)
                .Include(_ => _.Product).ToListAsync();
        }

        public CreateOrderDetailModel GetCreateOrderDetailModel(int orderId)
        {
            var model = new CreateOrderDetailModel();
            model.Order = _context.Orders.Include(_ => _.Customer).Include(_ => _.Department)
                .FirstOrDefault(_ => _.Id == orderId);
            model.Products = _context.Products.ToList();
            model.OrderDetails = _context.OrderDetails.Include(_ => _.Product)
                .Include(_ => _.Order).Where(_ => _.OrderId == orderId).ToList();
            model.OrderDetail = new();
            return model;
        }

        public DetailOrderDetailModel GetDetailOrderDetailModel(int id)
        {
            var model = new DetailOrderDetailModel();
            return new DetailOrderDetailModel
            {
                OrderDetails = _context.OrderDetails.Include(_ => _.Product)
                    .Include(_ => _.Order).Where(_ => _.OrderId == id).ToList(),
                Order = _context.Orders.Include(_ => _.Customer).Include(_ => _.Department)
                    .FirstOrDefault(_ => _.Id == id),
                OrderDetail = _context.OrderDetails.Include(_ => _.Product)
                    .Include(_ => _.Order).FirstOrDefault(_ => _.OrderId == id)
            };
        }

        public UpdateOrderDetailModel GetUpdateOrderDetailModel(int id)
        {
            var model = new UpdateOrderDetailModel();
            model.Products = _context.Products.ToList();
            model.Order = _context.Orders.Include(_ => _.Customer).Include(_ => _.Department)
                .FirstOrDefault(_ => _.Id == id);
            model.OrderDetails = _context.OrderDetails.Include(_ => _.Product)
                .Include(_ => _.Order).Where(_ => _.Id == id).ToList();
            model.OrderDetail = _context.OrderDetails.Include(_ => _.Product)
                .Include(_ => _.Order).FirstOrDefault(_ => _.Id == id);
            return model;
        }
    }
}

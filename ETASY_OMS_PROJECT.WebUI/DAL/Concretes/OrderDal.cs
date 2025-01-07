using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.DAL.Concretes.Data;
using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Concrete;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using ETASY_OMS_PROJECT.WebUI.Entity.ViewModels.OrderVM;
using Microsoft.EntityFrameworkCore;

namespace ETASY_OMS_PROJECT.WebUI.DAL.Concretes
{
    public class OrderDal : GenericRepository<Order>, IOrderDal
    {
        private readonly ApplicationDbContext _context;

        public OrderDal(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders.Include(_ => _.Customer)
                .Include(_ => _.Department).ToListAsync();
        }

        public override Order Get(Guid id)
        {
            return _context.Orders.Include(_ => _.Customer)
                .Include(_ => _.Department).FirstOrDefault();
        }

        public async Task<bool> CheckFormIdAsync(int formId)
        {
            return await _context.Orders.Include(_ => _.Customer)
                .Include(_ => _.Department).AnyAsync(_ => _.FormId == formId);
        }

        public async Task<bool> CheckFormIdAsync(Guid id, int formId)
        {
            return await _context.Orders.Include(_ => _.Customer)
                .Include(_ => _.Department).Where(_ => _.Id != id)
                .AnyAsync(_ => _.FormId == formId);
        }

        public CreateOrderModel GetCreateOrderModel()
        {
            return new CreateOrderModel
            {
                Order = new(),
                Customers = _context.Customers.ToList()
            };
        }

        public UpdateOrderModel GetUpdateOrderModel(Guid id)
        {
            return new UpdateOrderModel
            {
                Order = _context.Orders.Include(_ => _.Customer)
                    .Include(_ => _.Department).FirstOrDefault(_ => _.Id == id),
                Customers = _context.Customers.ToList()
            };
        }

        public DetailOrderModel GetDetailOrderModel(Guid id)
        {
            return new DetailOrderModel
            {
                Order = _context.Orders.Include(_ => _.Customer)
                    .Include(_ => _.Department).FirstOrDefault(),
                Products = _context.Products.ToList(),
                OrderDetails = _context.OrderDetails.Include(_ => _.Product).Where(_ => _.OrderId == id).ToList()
            };
        }
    }
}

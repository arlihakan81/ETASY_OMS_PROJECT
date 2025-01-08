using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.DAL.Concretes.Data;
using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Concrete;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using ETASY_OMS_PROJECT.WebUI.Entity.ViewModels.WarehouseVM;
using Microsoft.EntityFrameworkCore;

namespace ETASY_OMS_PROJECT.WebUI.DAL.Concretes
{
    public class WarehouseDal : GenericRepository<Warehouse>, IWarehouseDal
    {
        private readonly ApplicationDbContext _context;

        public WarehouseDal(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<List<Warehouse>> GetAllAsync()
        {
            return await _context.Warehouses.Include(_ => _.Supplier).ToListAsync();
        }


        public CreateWarehouseModel GetCreateWarehouseModel()
        {
            return new CreateWarehouseModel
            {
                Suppliers = _context.Suppliers.ToList(),
                Warehouse = new()
            };
        }

        public UpdateWarehouseModel GetUpdateWarehouseModel(Guid id)
        {
            return new UpdateWarehouseModel
            {
                Suppliers = _context.Suppliers.ToList(),
                Warehouse = _context.Warehouses.Include(_ => _.Supplier).Where(_ => _.Id == id).FirstOrDefault()
            };
        }
    }
}

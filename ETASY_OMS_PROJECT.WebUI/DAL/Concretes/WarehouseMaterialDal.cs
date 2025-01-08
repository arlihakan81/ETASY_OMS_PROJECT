using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.DAL.Concretes.Data;
using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Concrete;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using ETASY_OMS_PROJECT.WebUI.Entity.ViewModels.WarehouseMaterialVM;
using Microsoft.EntityFrameworkCore;

namespace ETASY_OMS_PROJECT.WebUI.DAL.Concretes
{
    public class WarehouseMaterialDal : GenericRepository<WarehouseMaterial>, IWarehouseMaterialDal
    {
        private readonly ApplicationDbContext _context;

        public WarehouseMaterialDal(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }

        public override async Task<List<WarehouseMaterial>> GetAllAsync()
        {
            return await _context.WarehouseMaterials.Include(_ => _.Material).Include(_ => _.Warehouse).ToListAsync();
        }

        public CreateWarehouseMaterialModel GetCreateWarehouseMaterialModel(Guid id)
        {
            return new CreateWarehouseMaterialModel
            {
                Warehouse = _context.Warehouses.Include(_ => _.Supplier).FirstOrDefault(_ => _.Id == id),
                Materials = _context.Materials.ToList(),
                WarehouseMaterials = _context.WarehouseMaterials.Include(_ => _.Material).Include(_ => _.Warehouse).ToList(),
                WarehouseMaterial = new()
            };
        }

        public UpdateWarehouseMaterialModel GetUpdateWarehouseMaterialModel(Guid id)
        {
            return new UpdateWarehouseMaterialModel
            {
                Warehouse = _context.Warehouses.Include(_ => _.Supplier).FirstOrDefault(_ => _.Id == id),
                Materials = _context.Materials.ToList(),
                WarehouseMaterial = _context.WarehouseMaterials.Include(_ => _.Material).Include(_ => _.Warehouse).FirstOrDefault(_ => _.Id == id)
            };
        }
    }
}

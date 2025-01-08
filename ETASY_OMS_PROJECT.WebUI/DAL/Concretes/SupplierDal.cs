using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.DAL.Concretes.Data;
using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Concrete;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETASY_OMS_PROJECT.WebUI.DAL.Concretes
{
    public class SupplierDal : GenericRepository<Supplier>, ISupplierDal
    {
        private readonly ApplicationDbContext _context;

        public SupplierDal(ApplicationDbContext context):base(context)
        {
            _context = context;
        }

        public async Task<bool> CheckSupplierCodeAsync(string code)
        {
            return await _context.Suppliers.AnyAsync(_ => _.Code.ToLower() == code.ToLower());
        }

        public async Task<bool> CheckSupplierCodeAsync(Guid id, string code)
        {
            return await _context.Suppliers.Where(_ => _.Id != id).AnyAsync(_ => _.Code.ToLower() == code.ToLower());
        }

        public async Task<bool> CheckSupplierNameAsync(string name)
        {
            return await _context.Suppliers.AnyAsync(_ => _.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> CheckSupplierNameAsync(Guid id, string name)
        {
            return await _context.Suppliers.Where(_ => _.Id != id).AnyAsync(_ => _.Name.ToLower() == name.ToLower());
        }
    }
}

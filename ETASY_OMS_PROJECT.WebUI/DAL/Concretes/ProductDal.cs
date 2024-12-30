using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.DAL.Concretes.Data;
using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Concrete;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETASY_OMS_PROJECT.WebUI.DAL.Concretes
{
    public class ProductDal : GenericRepository<Product>, IProductDal
    {
        private readonly ApplicationDbContext _context;

        public ProductDal(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> CheckCodeAsync(string code)
        {
            return await _context.Products.AnyAsync(_ => _.Code.ToLower() == code.ToLower());
        }

        public async Task<bool> CheckCodeAsync(int id, string code)
        {
            return await _context.Products.Where(_ => _.Id != id)
                .AnyAsync(_ => _.Code.ToLower() == code.ToLower());
        }

        public async Task<bool> CheckNameAsync(string name)
        {
            return await _context.Products.AnyAsync(_ => _.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> CheckNameAsync(int id, string name)
        {
            return await _context.Products.Where(_ => _.Id != id)
                .AnyAsync(_ => _.Name.ToLower() == name.ToLower());
        }
    }
}

using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.DAL.Concretes.Data;
using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Concrete;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETASY_OMS_PROJECT.WebUI.DAL.Concretes
{
    public class CustomerDal : GenericRepository<Customer>, ICustomerDal
    {
        private readonly ApplicationDbContext _context;

        public CustomerDal(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<bool> CheckNameAsync(string name)
        {
            return await _context.Customers.AnyAsync(_ => _.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> CheckNameAsync(int id, string name)
        {
            return await _context.Customers.Where(_ => _.Id != id)
                .AnyAsync(_ => _.Name.ToLower() == name.ToLower());
        }
    }
}

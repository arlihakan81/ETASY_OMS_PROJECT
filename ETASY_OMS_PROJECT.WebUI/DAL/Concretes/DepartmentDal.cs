using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.DAL.Concretes.Data;
using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Concrete;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETASY_OMS_PROJECT.WebUI.DAL.Concretes
{
    public class DepartmentDal : GenericRepository<Department>, IDepartmentDal
    {
        private readonly ApplicationDbContext _context;

        public DepartmentDal(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> CheckDepartmentNameAsync(string name)
        {
            return await _context.Departments.AnyAsync(_ => _.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> CheckDepartmentNameAsync(Guid id, string name)
        {
            return await _context.Departments.Where(_ => _.Id != id).AnyAsync(_ => _.Name.ToLower() == name.ToLower());
        }
    }
}

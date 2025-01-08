using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.DAL.Concretes.Data;
using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Concrete;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETASY_OMS_PROJECT.WebUI.DAL.Concretes
{
    public class MaterialDal : GenericRepository<Material>, IMaterialDal
    {
        private readonly ApplicationDbContext _context;

        public MaterialDal(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> CheckMaterialCodeAsync(string code)
        {
            return await _context.Materials.AnyAsync(_ => _.Code.ToLower() == code.ToLower());
        }

        public async Task<bool> CheckMaterialCodeAsync(Guid id, string code)
        {
            return await _context.Materials.Where(_ => _.Id != id).AnyAsync(_ => _.Code.ToLower() == code.ToLower());
        }

        public async Task<bool> CheckMaterialNameAsync(string name)
        {
            return await _context.Materials.AnyAsync(_ => _.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> CheckMaterialNameAsync(Guid id, string name)
        {
            return await _context.Materials.Where(_ => _.Id != id).AnyAsync(_ => _.Name.ToLower() == name.ToLower());
        }
    }
}

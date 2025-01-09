using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.DAL.Concretes.Data;
using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Concrete;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETASY_OMS_PROJECT.WebUI.DAL.Concretes
{
    public class GroupDal : GenericRepository<Group>, IGroupDal
    {
        private readonly ApplicationDbContext _context;

        public GroupDal(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> CheckGroupCodeAsync(string code)
        {
            return await _context.Groups.AnyAsync(_ => _.Code.ToLower() == code.ToLower());
        }

        public async Task<bool> CheckGroupCodeAsync(Guid id, string code)
        {
            return await _context.Groups.Where(_ => _.Id != id).AnyAsync(_ => _.Code.ToLower() == code.ToLower());
        }
    }
}

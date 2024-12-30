using ETASY_OMS_PROJECT.WebUI.Business.Abstracts;
using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.DAL.Concretes.Data;
using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Concrete;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using ETASY_OMS_PROJECT.WebUI.Entity.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ETASY_OMS_PROJECT.WebUI.DAL.Concretes
{
    public class AccountDal : GenericRepository<User>, IAccountDal
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordService _service;

        public AccountDal(ApplicationDbContext context, IPasswordService service) : base(context)
        {
            _context = context;
            _service = service;
        }

        public async Task<User> CheckAccountAsync(LoginViewModel model)
        {
            return await _context.Users.Include(_ => _.Department)
                .Where(_ => _.Name == model.Username && _.Password == _service.HashPassword(model.Password))
                .SingleOrDefaultAsync();
        }

        public async Task<bool> CheckResetAsync(int id, ResetViewModel model)
        {
            return await _context.Users.Include(_ => _.Department)
                .Where(_ => _.Id == id)
                .AnyAsync(_ => _.Password == _service.HashPassword(model.CurrentPassword));
        }

        public async Task<bool> CheckUsernameAsync(string Username)
        {
            return await _context.Users.Include(_ => _.Department)
                .AnyAsync(_ => _.Name.ToLower() == Username.ToLower());
        }

        public async Task<bool> CheckUsernameAsync(int id, string Username)
        {
            return await _context.Users.Include(_ => _.Department).Where(_ => _.Id != id)
                .AnyAsync(_ => _.Name.ToLower() == Username.ToLower());
        }
    }
}

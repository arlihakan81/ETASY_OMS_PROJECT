using ETASY_OMS_PROJECT.WebUI.DAL.Concretes.Data;
using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Abstract;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace ETASY_OMS_PROJECT.WebUI.DAL.Repos.Concrete
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> 
            where TEntity : EntityBase
    {
        private readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            await SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            _context.Set<TEntity>().Remove(Get(id));
            await SaveAsync();
        }

        public TEntity Get(int id)
        {
            return _context.Set<TEntity>().Where(_ => _.Id == id).SingleOrDefault();
        }

        public Task<List<TEntity>> GetAllAsync()
        {
            return _context.Set<TEntity>().ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            await SaveAsync();
        }
    }
}

using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.DAL.Concretes.Data;
using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Concrete;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;

namespace ETASY_OMS_PROJECT.WebUI.DAL.Concretes
{
    public class DepartmentDal : GenericRepository<Department>, IDepartmentDal
    {
        private readonly ApplicationDbContext _context;

        public DepartmentDal(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

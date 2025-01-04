using ETASY_OMS_PROJECT.WebUI.DAL.Concretes.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ETASY_OMS_PROJECT.WebUI.Entity.ViewModels;
using Microsoft.EntityFrameworkCore;
namespace ETASY_OMS_PROJECT.WebUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var notify = _context.Notifications.ToList();
            return View(new IndexViewModel
            {
                Users = _context.Users.Include(_ => _.Department).ToList(),
                Orders = _context.Orders.Include(_ => _.Customer).Include(_ => _.Department).ToList(),
                Products = _context.Products.ToList(),
                Customers = _context.Customers.ToList(),
                Departments = _context.Departments.ToList(),
                Notifications = _context.Notifications.ToList()
            });
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}

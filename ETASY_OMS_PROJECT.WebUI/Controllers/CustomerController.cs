using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using ETASY_OMS_PROJECT.WebUI.Entity.Enums.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ETASY_OMS_PROJECT.WebUI.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ICustomerDal _customer;
        private readonly INotificationDal _notification;

        public CustomerController(ICustomerDal customer, INotificationDal notification)
        {
            _customer = customer;
            _notification = notification;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _customer.GetAllAsync());
        }

        [Authorize(Roles = "ExportUser,DomesticUser")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "ExportUser,DomesticUser")]
        [HttpPost]
        public async Task<IActionResult> Create(Customer model)
        {
            if(ModelState.IsValid)
            {
                if(!await _customer.CheckNameAsync(model.Name))
                {
                    await _customer.AddAsync(new Customer
                    {
                        Name = model.Name,
                        Address = model.Address,
                        CreatedAt = DateTime.Now
                    });
                    TempData["success"] = "Yeni müşteri başarılı bir şekilde eklendi";
                    await _notification.AddAsync(new Notification
                    {
                        Operation = Operation.Customer_Create,
                        Description = $"{User.Identity.Name} isimli kullanıcı {DateTime.Now} itibariyle yeni müşteri ekledi.",
                        UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                        CreatedAt = DateTime.Now
                    });

                    return RedirectToAction(nameof(Create));
                }
                else
                {
                    TempData["error"] = "Bu müşteri adı kullanılıyor";
                    return View(model);
                }
            }
            else
            {
                TempData["error"] = "Müşteri adı ve adres bilgisi boş geçilemez";
                return View(model);
            }
        }

        [Authorize(Roles = "ExportUser,DomesticUser")]
        [HttpGet]
        public IActionResult Update(int id)
        {
            return View(_customer.Get(id));
        }

        [Authorize(Roles = "ExportUser,DomesticUser")]
        [HttpPost]
        public async Task<IActionResult> Update(int id, Customer model)
        {
            if(ModelState.IsValid)
            {
                if(!await _customer.CheckNameAsync(id, model.Name))
                {
                    var customer = _customer.Get(id);
                    customer.Name = model.Name;
                    customer.Address = model.Address;
                    customer.CreatedAt = customer.CreatedAt;
                    model.UpdatedAt = DateTime.Now;
                    TempData["success"] = "Müşteri bilgileri başarılı bir şekilde güncellendi";
                    await _notification.AddAsync(new Notification
                    {
                        Operation = Operation.Customer_Update,
                        Description = $"{User.Identity.Name} isimli kullanıcı {DateTime.Now} itibariyle bir müşteri kaydını güncelledi.",
                        UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                        CreatedAt = DateTime.Now
                    });
                    return RedirectToAction("Update", "Customer", new { id });
                }
                else
                {
                    TempData["error"] = "Bu müşteri adı kullanılıyor";
                    return View(model);
                }
            }
            else
            {
                TempData["error"] = "Müşteri adı ve adres bilgisi boş geçilemez";
                return View(model);
            }
        }

        [Authorize(Roles = "ExportUser,DomesticUser")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _customer.DeleteAsync(id);
            await _notification.AddAsync(new Notification
            {
                Operation = Operation.Customer_Create,
                Description = $"{User.Identity.Name} isimli kullanıcı {DateTime.Now} itibariyle bir müşteri kaydını sildi.",
                UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                CreatedAt = DateTime.Now
            });
            return RedirectToAction(nameof(Index));
        }


    }
}

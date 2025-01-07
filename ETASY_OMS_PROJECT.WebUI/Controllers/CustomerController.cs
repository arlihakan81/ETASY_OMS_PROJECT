using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using ETASY_OMS_PROJECT.WebUI.Entity.Enums.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ETASY_OMS_PROJECT.WebUI.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ICustomerDal _customer;
        private readonly INotificationDal _notification;
        private readonly INotifyUserDal _notifyUser;
        private readonly IAccountDal _account;

        public CustomerController(ICustomerDal customer, INotificationDal notification, IAccountDal account, INotifyUserDal notifyUser)
        {
            _customer = customer;
            _notification = notification;
            _account = account;
            _notifyUser = notifyUser;
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
                    var notification = new Notification
                    {
                        Operation = Operation.Customer_Create,
                        Description = $"{User.Identity.Name} isimli kullanıcı {DateTime.Now} itibariyle yeni müşteri ekledi.",
                        UserId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                        CreatedAt = DateTime.Now
                    };
                    await _notification.AddAsync(notification);

                    foreach(var item in await _account.GetAllAsync())
                    {
                        await _notifyUser.AddAsync(new NotifyUser
                        {
                            UserId = item.Id,
                            NotificationId = notification.Id,
                            IsRead = false,
                            CreatedAt = DateTime.Now
                        });
                    }

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
        public IActionResult Update(Guid id)
        {
            return View(_customer.Get(id));
        }

        [Authorize(Roles = "ExportUser,DomesticUser")]
        [HttpPost]
        public async Task<IActionResult> Update(Guid id, Customer model)
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
                    await _customer.UpdateAsync(customer);
                    TempData["success"] = "Müşteri bilgileri başarılı bir şekilde güncellendi";
                    var notification = new Notification
                    {
                        Operation = Operation.Customer_Update,
                        Description = $"{User.Identity.Name} isimli kullanıcı {DateTime.Now} itibariyle bir müşteri kaydını güncelledi.",
                        UserId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                        CreatedAt = DateTime.Now
                    };
                    await _notification.AddAsync(notification);

                    foreach (var item in await _account.GetAllAsync())
                    {
                        await _notifyUser.AddAsync(new NotifyUser
                        {
                            UserId = item.Id,
                            NotificationId = notification.Id,
                            IsRead = false,
                            CreatedAt = DateTime.Now
                        });
                    }

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
        public async Task<IActionResult> Delete(Guid id)
        {
            await _customer.DeleteAsync(id);
            var notification = new Notification
            {
                Operation = Operation.Customer_Create,
                Description = $"{User.Identity.Name} isimli kullanıcı {DateTime.Now} itibariyle bir müşteri kaydını sildi.",
                UserId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                CreatedAt = DateTime.Now
            };
            await _notification.AddAsync(notification);

            foreach (var item in await _account.GetAllAsync())
            {
                await _notifyUser.AddAsync(new NotifyUser
                {
                    UserId = item.Id,
                    NotificationId = notification.Id,
                    IsRead = false,
                    CreatedAt = DateTime.Now
                });
            }

            return RedirectToAction(nameof(Index));
        }


    }
}

using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using ETASY_OMS_PROJECT.WebUI.Entity.Enums.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ETASY_OMS_PROJECT.WebUI.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductDal _product;
        private readonly INotificationDal _notification;
        private readonly IAccountDal _account;
        private readonly INotifyUserDal _notifyUser;

        public ProductController(IProductDal product, INotificationDal notification, IAccountDal account, INotifyUserDal notifyUser)
        {
            _product = product;
            _notification = notification;
            _account = account;
            _notifyUser = notifyUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _product.GetAllAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Quality")]
        [HttpPost]
        public async Task<IActionResult> Create(Product model)
        {
            if(ModelState.IsValid)
            {
                if(!await _product.CheckNameAsync(model.Name))
                {
                    if (!await _product.CheckCodeAsync(model.Code))
                    {
                        await _product.AddAsync(new Product
                        {
                            Code = model.Code,
                            Name = model.Name,
                            Scale = model.Scale,
                            CreatedAt = DateTime.Now
                        });
                        TempData["success"] = "Yeni ürün başarılı bir şekilde eklendi";
                        var notification = new Notification
                        {
                            Operation = Operation.Product_Create,
                            Description = $"{User.Identity.Name} isimli kullanıcı {DateTime.Now} itibariyle yeni bir ürün kaydı ekledi.",
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

                        return RedirectToAction("Create", "Product");
                    }
                    else
                    {
                        TempData["error"] = "Bu ürün kodu kullanılıyor";
                        return View(model);
                        
                    }
                }
                else
                {
                    TempData["error"] = "Bu ürün adı kullanılıyor";
                    return View(model);
                }
            }
            else
            {
                TempData["error"] = "Ürün eklemek için ürün bilgilerini girin";
                return View(model);
            }
        }

        [Authorize(Roles = "Quality")]
        [HttpGet]
        public IActionResult Update(Guid id)
        {
            return View(_product.Get(id));
        }

        [Authorize(Roles = "Quality")]
        [HttpPost]
        public async Task<IActionResult> Update(Guid id, Product model)
        {
            if(ModelState.IsValid)
            {
                var product = _product.Get(id);
                product.Name = model.Name;
                product.Code = model.Code;
                product.Scale = model.Scale;
                product.CreatedAt = model.CreatedAt;
                product.UpdatedAt = DateTime.Now;
                await _product.UpdateAsync(product);
                TempData["success"] = "Ürün başarılı bir şekilde güncellendi";
                var notification = new Notification
                {
                    Operation = Operation.Product_Update,
                    Description = $"{User.Identity.Name} isimli kullanıcı {DateTime.Now} itibariyle bir ürün kaydını güncelledi.",
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

                return RedirectToAction(nameof(Update));
            }
            else
            {
                TempData["error"] = "Ürün bilgilerini girin.";
                return View(model);
            }
        }

        [Authorize(Roles = "Quality")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _product.DeleteAsync(id);
            var notification = new Notification
            {
                Operation = Operation.Product_Delete,
                Description = $"{User.Identity.Name} isimli kullanıcı {DateTime.Now} itibariyle bir sipariş kaydını sildi.",
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

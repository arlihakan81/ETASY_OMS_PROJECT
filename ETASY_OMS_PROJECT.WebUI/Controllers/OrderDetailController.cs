using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.Entity.ViewModels.OrderDetailVM;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using ETASY_OMS_PROJECT.WebUI.Entity.Enums.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ETASY_OMS_PROJECT.WebUI.Controllers
{
    [Authorize]
    public class OrderDetailController : Controller
    {
        private readonly IOrderDetailDal _detail;
        private readonly INotificationDal _notification;
        private readonly IAccountDal _account;
        private readonly INotifyUserDal _notifyUser;

        public OrderDetailController(IOrderDetailDal detail, INotificationDal notification, IAccountDal account, INotifyUserDal notifyUser) 
        {
            _detail = detail;
            _notification = notification;
            _account = account;
            _notifyUser = notifyUser;
        }

        [Authorize(Roles = "ExportUser,DomesticUser")]
        [HttpGet]
        public IActionResult Create(int id)
        {
            return View(_detail.GetCreateOrderDetailModel(id));
        }

        [Authorize(Roles = "ExportUser,DomesticUser")]
        [HttpPost]
        public async Task<IActionResult> Create(int id, CreateOrderDetailModel model)
        {
            if(ModelState.IsValid)
            {
                await _detail.AddAsync(new OrderDetail
                {
                    OrderId = id,
                    ProductId = model.OrderDetail.ProductId,
                    Quantity = model.OrderDetail.Quantity,
                    CreatedAt = DateTime.Now
                });
                TempData["success"] = $"{model.Order.FormId} No'lu Siparişinize yeni bir ürün eklendi.";
                var notification = new Notification
                {
                    Operation = Operation.Order_Create,
                    Description = $"{User.Identity.Name} isimli kullanıcı {DateTime.Now} itibariyle yeni bir sipariş kaydı ekledi.",
                    UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
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

                return RedirectToAction("Create", "OrderDetail", new { model.OrderDetail.OrderId });
            }
            else
            {
                TempData["error"] = "Sipariş boş geçilemez!";
                return View(model);
            }
        }

        [Authorize(Roles = "ExportUser,DomesticUser")]
        [HttpGet]
        public IActionResult Update(int id)
        {
            return View(_detail.GetUpdateOrderDetailModel(id));
        }

        [Authorize(Roles = "ExportUser,DomesticUser")]
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateOrderDetailModel model)
        {
            if (ModelState.IsValid)
            {
                var detail = _detail.Get(id);
                detail.OrderId = model.OrderDetail.OrderId;
                detail.ProductId = model.OrderDetail.ProductId;
                detail.Quantity = model.OrderDetail.Quantity;
                detail.CreatedAt = model.OrderDetail.CreatedAt;
                detail.UpdatedAt = DateTime.Now;
                await _detail.UpdateAsync(detail);
                TempData["success"] = $"{model.Order.FormId} No'lu Siparişinizdeki bir ürün güncellendi.";
                var notification = new Notification
                {
                    Operation = Operation.Order_Update,
                    Description = $"{User.Identity.Name} isimli kullanıcı {DateTime.Now} itibariyle bir sipariş kaydını güncelledi.",
                    UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
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

                return RedirectToAction("Update", "OrderDetail", new { model.OrderDetail.Id });
            }
            else
            {
                TempData["error"] = "Sipariş boş geçilemez!";
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Detail(int id) // Order.Id
        {
            return View(_detail.GetDetailOrderDetailModel(id));
        }

        [Authorize(Roles = "ExportUser,DomesticUser")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id) // OrderDetail.Id
        {
            await _detail.DeleteAsync(id);
            var notification = new Notification
            {
                Operation = Operation.Order_Delete,
                Description = $"{User.Identity.Name} isimli kullanıcı {DateTime.Now} itibariyle bir sipariş kaydını sildi.",
                UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
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

            return RedirectToAction("Detail", "OrderDetail");
        }


    }
}

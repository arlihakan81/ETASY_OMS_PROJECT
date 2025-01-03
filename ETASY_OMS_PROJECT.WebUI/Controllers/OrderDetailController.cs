using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.Entity.ViewModels.OrderDetailVM;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using ETASY_OMS_PROJECT.WebUI.Entity.Enums.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ETASY_OMS_PROJECT.WebUI.Controllers
{
    [Authorize]
    public class OrderDetailController : Controller
    {
        private readonly IOrderDetailDal _detail;
        private readonly INotificationDal _notification;
        public OrderDetailController(IOrderDetailDal detail, INotificationDal notification) 
        {
            _detail = detail;
            _notification = notification;
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
                await _notification.AddAsync(new Notification
                {
                    Operation = Operation.Order_Create,
                    Description = $"{User.Identity.Name} isimli kullanıcı {DateTime.Now} itibariyle yeni bir sipariş kaydı ekledi.",
                    UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    IsRead = false,
                    CreatedAt = DateTime.Now
                });
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
                await _detail.AddAsync(new OrderDetail
                {
                    OrderId = id,
                    ProductId = model.OrderDetail.ProductId,
                    Quantity = model.OrderDetail.Quantity,
                    CreatedAt = DateTime.Now
                });
                TempData["success"] = $"{model.Order.FormId} No'lu Siparişinize yeni bir ürün eklendi.";
                await _notification.AddAsync(new Notification
                {
                    Operation = Operation.Order_Update,
                    Description = $"{User.Identity.Name} isimli kullanıcı {DateTime.Now} itibariyle bir sipariş kaydını güncelledi.",
                    UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    IsRead = false,
                    CreatedAt = DateTime.Now
                });
                return RedirectToAction("Create", "OrderDetail", new { model.OrderDetail.OrderId });
            }
            else
            {
                TempData["error"] = "Sipariş boş geçilemez!";
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            return View(_detail.GetDetailOrderDetailModel(id));
        }

        [Authorize(Roles = "ExportUser,DomesticUser")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _detail.DeleteAsync(id);
            await _notification.AddAsync(new Notification
            {
                Operation = Operation.Order_Delete,
                Description = $"{User.Identity.Name} isimli kullanıcı {DateTime.Now} itibariyle bir sipariş kaydını sildi.",
                UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                IsRead = false,
                CreatedAt = DateTime.Now
            });
            return RedirectToAction("Detail", "OrderDetail");
        }


    }
}

using System.Security.Claims;
using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using ETASY_OMS_PROJECT.WebUI.Entity.Enums.Notifications;
using ETASY_OMS_PROJECT.WebUI.Entity.ViewModels.OrderVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETASY_OMS_PROJECT.WebUI.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderDal _order;
        private readonly INotificationDal _notification;
        public OrderController(IOrderDal order, INotificationDal notification)
        {
            _order = order;
            _notification = notification;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _order.GetAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Delivered()
        {
            return View(await _order.GetAllAsync());
        }

        [Authorize(Roles = "ExportUser,DomesticUser")]
        [HttpGet]
        public IActionResult Create()
        {
            return View(_order.GetCreateOrderModel());
        }

        [Authorize(Roles = "ExportUser,DomesticUser")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderModel model)
        {
            if (ModelState.IsValid)
            {
                if(!await _order.CheckFormIdAsync(model.Order.FormId))
                {
                    var order = new Order
                    {
                        FormId = model.Order.FormId,
                        CustomerId = model.Order.CustomerId,
                        DueDate = model.Order.DueDate,
                        Status = model.Order.Status,
                        DepartmentId = int.Parse(User.FindFirstValue("DepartmentId")),
                        CreatedAt = DateTime.Now
                    };
                    await _order.AddAsync(order);
                    await _notification.AddAsync(new Notification
                    {
                        Operation = Operation.Order_Create,
                        Description = $"{User.Identity.Name} isimli kullanıcı {DateTime.Now} itibariyle yeni bir sipariş formu ekledi",
                        ReaderId = 0,
                        UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                        CreatedAt = DateTime.Now
                    });
                    TempData["success"] = "Yeni sipariş formu eklendi siparişi kaydetmek için lütfen ürün ve adetini girin.";
                    return RedirectToAction("Create", "OrderDetail", new {order.Id});
                }
                else
                {
                    TempData["error"] = "Bu form numarası kullanılıyor";
                    return View(model);
                }
            }
            else
            {
                TempData["error"] = "Sipariş bilgilerini girin";
                return View(model);
            }
        }

        [Authorize(Roles = "ExportUser,DomesticUser")]
        [HttpGet]
        public IActionResult Update(int id)
        {
            return View(_order.GetUpdateOrderModel(id));
        }

        [Authorize(Roles = "ExportUser,DomesticUser")]
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateOrderModel model)
        {
            if (ModelState.IsValid)
            {
                var ord = _order.Get(id);
                ord.FormId = model.Order.FormId;
                ord.CustomerId = model.Order.CustomerId;
                ord.DueDate = model.Order.DueDate;
                ord.Status = model.Order.Status;
                ord.DepartmentId = int.Parse(User.FindFirstValue("DepartmentId"));
                ord.CreatedAt = model.Order.CreatedAt;
                ord.UpdatedAt = DateTime.Now;
                await _order.UpdateAsync(ord);
                await _notification.AddAsync(new Notification
                {
                    Operation = Operation.Order_Update,
                    Description = $"{User.Identity.Name} isimli kullanıcı {DateTime.Now} itibariyle bir sipariş formunu güncelledi",
                    ReaderId = 0,
                    UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    CreatedAt = DateTime.Now
                });
                TempData["success"] = "Sipariş başarılı bir şekilde güncellendi";
                return RedirectToAction("Update", "Order", new {id});
                
            }
            else
            {
                TempData["error"] = "Sipariş bilgilerini girin";
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            return View(_order.GetDetailOrderModel(id));
        }

        [Authorize(Roles = "ExportUser,DomesticUser")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _order.DeleteAsync(id);
            await _notification.AddAsync(new Notification
            {
                Operation = Operation.Order_Delete,
                Description = $"{User.Identity.Name} isimli kullanıcı {DateTime.Now} itibariyle bir sipariş formunu sildi",
                ReaderId = 0,
                UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                CreatedAt = DateTime.Now
            });
            return RedirectToAction(nameof(Index));
        }


    }
}

using System.Security.Claims;
using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.Entity.Enums.Notifications;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ETASY_OMS_PROJECT.WebUI.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationDal _notification;

        public NotificationController(INotificationDal notification)
        {
            _notification = notification;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(await _notification.GetAllAsync(id));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Notification model)
        {
            if(ModelState.IsValid)
            {
                await _notification.AddAsync(new Notification
                {
                    Operation = model.Operation,
                    Description = $"{User.Identity.Name} isimli kullanıcı şu işlemi yaptı: {model.Operation}",
                    UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    CreatedAt = DateTime.Now
                });
                TempData["success"] = "Yeni bildirim oluşturuldu";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Gerekli bilgileri girin";
                return View(model);
            }
        }

    }
}

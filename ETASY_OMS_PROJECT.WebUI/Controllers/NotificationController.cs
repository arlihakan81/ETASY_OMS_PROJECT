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

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var notify = _notification.Get(id);
            notify.IsRead = true;
            await _notification.UpdateAsync(notify);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _notification.DeleteAsync(id);
            return RedirectToAction("Index");
        }



    }
}

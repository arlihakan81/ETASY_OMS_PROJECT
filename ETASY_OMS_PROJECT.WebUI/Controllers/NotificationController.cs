using System.Security.Claims;
using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
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
        public async Task<IActionResult> Delete(int id)
        {
            await _notification.DeleteAsync(id);
            return RedirectToAction("Index");
        }



    }
}

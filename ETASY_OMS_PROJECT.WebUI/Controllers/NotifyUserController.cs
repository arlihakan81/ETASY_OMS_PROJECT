using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ETASY_OMS_PROJECT.WebUI.Controllers
{
    public class NotifyUserController : Controller
    {
        private readonly INotifyUserDal _notifyUser;

        public NotifyUserController(INotifyUserDal notifyUser)
        {
            _notifyUser = notifyUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _notifyUser.GetAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Read(Guid id)
        {
            var notify = _notifyUser.Get(id);
            notify.UserId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            notify.IsRead = true;
            await _notifyUser.UpdateAsync(notify);
            return RedirectToAction("Index", "NotifyUser");
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            foreach (var notify in await _notifyUser.GetAllAsync())
            {
                notify.IsRead = true;
                await _notifyUser.UpdateAsync(notify);
            }
            return RedirectToAction("Index", "NotifyUser");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAll()
        {
            foreach (var notify in await _notifyUser.GetAllAsync())
            {
                if(notify.User.Name == User.Identity.Name)
                {
                    await _notifyUser.DeleteAsync(notify.Id);
                }
            }
            return RedirectToAction("Index", "NotifyUser");
        }

    }
}

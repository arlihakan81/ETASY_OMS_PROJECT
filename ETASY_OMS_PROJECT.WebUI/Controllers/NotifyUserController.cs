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
        public async Task<IActionResult> Read(int id)
        {
            var notify = _notifyUser.Get(id);
            notify.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            notify.IsRead = true;
            await _notifyUser.UpdateAsync(notify);
            return RedirectToAction("Index", "Home");
        }
    }
}

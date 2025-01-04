using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.Entity.ViewModels.AccountVM;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ETASY_OMS_PROJECT.WebUI.Business.Abstracts;

namespace ETASY_OMS_PROJECT.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IAccountDal _account;
        private readonly IPasswordService _service;
        private readonly IWebHostEnvironment _webHost;

        public UserController(IAccountDal account, IPasswordService service, IWebHostEnvironment webHost)
        {
            _account = account;
            _service = service;
            _webHost = webHost;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _account.GetAllAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(_account.GetCreateAccountModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAccountModel model)
        {
            if(ModelState.IsValid)
            {
                if(!await _account.CheckUsernameAsync(model.User.Name))
                {
                    var filePath = Path.Combine(_webHost.WebRootPath, "dist/img/avatars");
                    if(!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    var fileName = Path.Combine(filePath, model.User.Image.FileName);
                    using (var stream = new FileStream(fileName, FileMode.Create))
                    {
                        await model.User.Image.CopyToAsync(stream);
                    }

                    await _account.AddAsync(new User
                    {
                        Avatar = model.User.Image.FileName,
                        Name = model.User.Name,
                        Password = _service.HashPassword(model.User.Password),
                        Role = model.User.Role,
                        DepartmentId = model.User.DepartmentId,
                        CreatedAt = DateTime.Now
                    });
                    TempData["success"] = "Yeni kullanıcı başarılı bir şekilde eklendi";
                    return RedirectToAction(nameof(Create));
                }
                else
                {
                    TempData["error"] = "Bu kullanıcı adı kullanılıyor";
                    return View(model);
                }
            }
            else
            {
                TempData["error"] = "Gerekli alanları doldurun";
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            return View(_account.GetUpdateAccountModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateAccountModel model)
        {
            if (ModelState.IsValid)
            {
                if (!await _account.CheckUsernameAsync(id, model.User.Name))
                {
                    var filePath = Path.Combine(_webHost.WebRootPath, "dist/img/avatars");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    var fileName = Path.Combine(filePath, model.User.Image.FileName);
                    using (var stream = new FileStream(fileName, FileMode.Create))
                    {
                        await model.User.Image.CopyToAsync(stream);
                    }
                    var user = _account.Get(id);
                    user.Avatar = model.User.Image.FileName;
                    user.Name = model.User.Name;
                    user.Password = model.User.Password;
                    user.Role = model.User.Role;
                    user.DepartmentId = model.User.DepartmentId;
                    user.CreatedAt = model.User.CreatedAt;
                    user.UpdatedAt = DateTime.Now;
                    await _account.UpdateAsync(user);
                    TempData["success"] = "Kullanıcı başarılı bir şekilde güncellendi";
                    return RedirectToAction("Update", "User", new {id});
                }
                else
                {
                    TempData["error"] = "Bu kullanıcı adı kullanılıyor";
                    return View(model);
                }
            }
            else
            {
                TempData["error"] = "Gerekli alanları doldurun";
                return View(model);
            }
        }
    }
}

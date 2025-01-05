using System.Security.Claims;
using ETASY_OMS_PROJECT.WebUI.Business.Abstracts;
using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.Entity.ViewModels;
using ETASY_OMS_PROJECT.WebUI.Entity.Enums.User;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using ETASY_OMS_PROJECT.WebUI.Entity.Enums.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace ETASY_OMS_PROJECT.WebUI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountDal _account;
        private readonly IPasswordService _service;
        private readonly IWebHostEnvironment _webHost;
        private readonly INotificationDal _notification;
        private readonly INotifyUserDal _notifyUser;

        public AccountController(IAccountDal account, IPasswordService service, 
            IWebHostEnvironment webHost, INotificationDal notification, INotifyUserDal notifyUser)
        {
            _account = account;
            _service = service;
            _webHost = webHost;
            _notification = notification;
            _notifyUser = notifyUser;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _account.CheckAccountAsync(model);
                if(user is not null)
                {
                    if(user.Role is not Role.Unauthorized)
                    {
                        List<Claim> claims = [];
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                        claims.Add(new Claim("Avatar", user.Avatar));
                        claims.Add(new Claim(ClaimTypes.Name, user.Name));
                        claims.Add(new Claim("Department", user.Department.Name));
                        claims.Add(new Claim("DepartmentId", user.DepartmentId.ToString()));
                        claims.Add(new Claim(ClaimTypes.Role, user.Role.ToString()));

                        ClaimsIdentity identity = new (claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        ClaimsPrincipal principal = new (identity);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                        var notification = new Notification
                        {
                            Operation = Operation.Account_Login,
                            Description = $"{user.Name} isimli kullanıcı {DateTime.Now} itibariyle oturum açtı.",
                            UserId = user.Id,
                            CreatedAt = DateTime.Now
                        };
                        await _notification.AddAsync(notification);

                        foreach(var item in await _account.GetAllAsync())
                        {
                            await _notifyUser.AddAsync(new NotifyUser
                            {
                                UserId = item.Id,
                                NotificationId = notification.Id,
                                IsRead = false,
                                CreatedAt = DateTime.Now
                            });
                        }

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["error"] = "Hesabınız henüz yetkilendirilmedi";
                        return View(model);
                    }
                }
                else
                {
                    TempData["error"] = "Kullanıcı adı veya parola hatalı";
                    return View(model);
                }
            }
            else
            {
                TempData["error"] = "Kullanıcı adı ve parola boş geçilemez!";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var notification = new Notification
            {
                Operation = Operation.Account_Logout,
                Description = $"{User.Identity.Name} isimli kullanıcı {DateTime.Now} itibariyle oturumunu sonlandırdı.",
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
            TempData["success"] = "Oturumunuz başarılı bir şekilde sonlandırıldı";
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                if(!await _account.CheckUsernameAsync(model.Username))
                {
                    var user = new User
                    {
                        Avatar = "default_avatar.png",
                        Name = model.Username,
                        Password = _service.HashPassword(model.Password),
                        Role = Role.Unauthorized,
                        DepartmentId = 1,
                        CreatedAt = DateTime.Now
                    };
                    await _account.AddAsync(user);

                    var notification = new Notification
                    {
                        Operation = Operation.Account_Register,
                        Description = $"{user.Name} isimli kullanıcı {DateTime.Now} itibariyle yeni üyelik açtı.",
                        UserId = user.Id,
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
                    TempData["success"] = "Üyelik kaydınız başarılı bir şekilde oluşturuldu";
                    return RedirectToAction(nameof(Register));
                }
                else
                {
                    TempData["error"] = "Bu kullanıcı adı zaten kayıtlı";
                    return View(model);
                }
            }
            else
            {
                TempData["error"] = "Üyelik oluşturmak için gerekli alanları doldurun";
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(_account.Get(id));
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, User model)
        {
            if (ModelState.IsValid)
            {
                id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (!await _account.CheckUsernameAsync(id, model.Name))
                {
                    var user = _account.Get(id);
                    var filePath = Path.Combine(_webHost.WebRootPath, "dist/img/avatars");
                    if(!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    var fileName = Path.Combine(filePath, model.Image.FileName);
                    using (var stream = new FileStream(fileName, FileMode.Create))
                    {
                        await model.Image.CopyToAsync(stream);
                    }
                    user.Avatar = model.Image.FileName;
                    user.Name = model.Name;
                    user.Password = model.Password;
                    user.Role = model.Role;
                    user.DepartmentId = model.DepartmentId;
                    user.CreatedAt = user.CreatedAt;
                    user.UpdatedAt = DateTime.Now;
                    await _account.UpdateAsync(user);
                    var notification = new Notification
                    {
                        Operation = Operation.Account_Register,
                        Description = $"{user.Name} isimli kullanıcı {DateTime.Now} itibariyle hesabını güncelledi.",
                        UserId = user.Id,
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

                    TempData["success"] = "Hesabınız başarılı bir şekilde güncellendi";
                    return RedirectToAction(nameof(Update));
                }
                else
                {
                    TempData["error"] = "Bu kullanıcı adı zaten kayıtlı";
                    return View(model);
                }
            }
            else
            {
                TempData["error"] = "Hesabınızı güncellemek için gerekli alanları doldurun";
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Reset(int id)
        {
            id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(new ResetViewModel
            {
                User = _account.Get(id)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Reset(int id, ResetViewModel model)
        {
            if(ModelState.IsValid)
            {
                id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (await _account.CheckResetAsync(id, model))
                {
                    var user = _account.Get(id);
                    if(model.NewPassword != _service.HashPassword(model.NewPassword))
                    {
                        user.Password = _service.HashPassword(model.NewPassword);
                        await _account.UpdateAsync(user);
                        TempData["success"] = "Parolanız başarılı bir şekilde güncellendi";
                        var notification = new Notification
                        {
                            Operation = Operation.Account_Reset,
                            Description = $"{user.Name} isimli kullanıcı {DateTime.Now} itibariyle parolasını değiştirdi.",
                            UserId = user.Id,
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

                        return RedirectToAction(nameof(Reset));
                    }
                    else 
                    {
                        TempData["error"] = "Yeni parola önceki parola ile aynı olamaz!";
                        return View(model);
                    }
                }
                else
                {
                    TempData["error"] = "Lütfen geçerli parolanızı girin";
                    return View(model);
                }
            }
            else
            {
                TempData["error"] = "Parola bilgileri boş geçilemez!";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> AccessDenied()
        {
            int id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var notification = new Notification
            {
                Operation = Operation.Access_Denied,
                Description = $"{User.Identity.Name} isimli kullanıcı {DateTime.Now} itibariyle erişim engeline takıldı.",
                UserId = id,
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

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = _account.Get(id);
            if(user.Role is not Role.Admin)
            {
                await _account.DeleteAsync(id);
                TempData["success"] = "Hesabınız başarılı bir şekilde silindi";
                var notification = new Notification
                {
                    Operation = Operation.Account_Delete,
                    Description = $"{user.Name} isimli kullanıcı {DateTime.Now} itibariyle hesabını sildi.",
                    UserId = user.Id,
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

                return RedirectToAction(nameof(Logout));
            }
            else
            {
                TempData["error"] = "Hesabınız admin hesabıdır. Silinemez";
                return RedirectToAction("Index", "Home");
            }
        }


    }
}

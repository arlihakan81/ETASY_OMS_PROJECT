using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETASY_OMS_PROJECT.WebUI.Controllers
{
    [Authorize(Roles = "HumanResources")]
    public class GroupController : Controller
    {
        private readonly IGroupDal _group;

        public GroupController(IGroupDal group)
        {
            _group = group;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _group.GetAllAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Group model)
        {
            if(ModelState.IsValid)
            {
                if(!await _group.CheckGroupCodeAsync(model.Code))
                {
                    var group = new Group
                    {
                        Code = model.Code,
                        Name = model.Name,
                        Description = model.Description,
                        IsExpense = model.IsExpense,
                        IsMonthly = model.IsMonthly,
                        Quantity = model.Quantity,
                        Amount = model.Amount,
                        CreatedAt = DateTime.Now
                    };
                    await _group.AddAsync(group);
                    TempData["success"] = "Gelir/Gider grubu eklendi";
                    return RedirectToAction(nameof(Create));
                }
                else
                {
                    TempData["error"] = "Bu Gelir/Gider kodu kullanılıyor!";
                    return View(model);
                }
            }
            else
            {
                TempData["error"] = "Gelir/Gider bilgilerini girin";
                return View(model);
            }
        }


        [HttpGet]
        public IActionResult Update(Guid id)
        {
            return View(_group.Get(id));
        }




    }
}

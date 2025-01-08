using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETASY_OMS_PROJECT.WebUI.Controllers
{
    [Authorize(Roles = "WarehouseController, WarehouseClient")]
    public class MaterialController : Controller
    {
        private readonly IMaterialDal _material;

        public MaterialController(IMaterialDal material)
        {
            _material = material;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _material.GetAllAsync());
        }

        [Authorize(Roles = "WarehouseController")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "WarehouseController")]
        [HttpPost]
        public async Task<IActionResult> Create(Material model)
        {
            if (ModelState.IsValid)
            {
                if(!await _material.CheckMaterialCodeAsync(model.Code))
                {
                    if(!await _material.CheckMaterialNameAsync(model.Name))
                    {
                        var material = new Material
                        {
                            Code = model.Code,
                            Name = model.Name,
                            Description = model.Description,
                            CreatedAt = DateTime.Now
                        };
                        await _material.AddAsync(material);
                        TempData["success"] = "Yeni malzeme kaydı başarılı bir şekilde eklendi";
                        return RedirectToAction("Index", "Material");
                    }
                    else
                    {
                        TempData["error"] = "Bu malzeme adı kullanılıyor!";
                        return View(model);
                    }
                }
                else
                {
                    TempData["error"] = "Bu malzeme kodu kullanılıyor!";
                    return View(model);
                }
            }
            else
            {
                TempData["error"] = "Gerekli alanları girin";
                return View(model);
            }
        }


        [Authorize(Roles = "WarehouseController")]
        [HttpGet]
        public IActionResult Update(Guid id)
        {
            return View(_material.Get(id));
        }

        [Authorize(Roles = "WarehouseController")]
        [HttpPost]
        public async Task<IActionResult> Update(Guid id, Material model)
        {
            if (ModelState.IsValid)
            {
                if (!await _material.CheckMaterialCodeAsync(id, model.Code))
                {
                    if (!await _material.CheckMaterialNameAsync(id, model.Name))
                    {
                        var material = _material.Get(id);
                        material.Code = model.Code;
                        material.Name = model.Name;
                        material.Description = model.Description;
                        material.CreatedAt = model.CreatedAt;
                        material.UpdatedAt = DateTime.Now;
                        await _material.UpdateAsync(material);
                        TempData["success"] = "Malzeme kaydı başarılı bir şekilde güncellendi";
                        return RedirectToAction("Update", "Material", new { id });
                    }
                    else
                    {
                        TempData["error"] = "Bu malzeme adı kullanılıyor!";
                        return View(model);
                    }
                }
                else
                {
                    TempData["error"] = "Bu malzeme kodu kullanılıyor!";
                    return View(model);
                }
            }
            else
            {
                TempData["error"] = "Gerekli alanları girin";
                return View(model);
            }
        }

        [Authorize(Roles = "WarehouseController")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _material.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }


    }
}

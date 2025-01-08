using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETASY_OMS_PROJECT.WebUI.Controllers
{
    [Authorize(Roles = "WarehouseController, WarehouseClient")]
    public class SupplierController : Controller
    {
        private readonly ISupplierDal _supplier;

        public SupplierController(ISupplierDal supplier)
        {
            _supplier = supplier;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _supplier.GetAllAsync());
        }

        [Authorize(Roles = "WarehouseController")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "WarehouseController")]
        [HttpPost]
        public async Task<IActionResult> Create(Supplier model)
        {
            if (ModelState.IsValid)
            {
                if(!await _supplier.CheckSupplierCodeAsync(model.Code))
                {
                    if(!await _supplier.CheckSupplierNameAsync(model.Name))
                    {
                        var supplier = new Supplier
                        {
                            Code = model.Code,
                            Name = model.Name,
                            Address = model.Address,
                            CreatedAt = DateTime.Now
                        };
                        await _supplier.AddAsync(supplier);
                        TempData["success"] = "Yeni tedarikçi kaydı başarılı bir şekilde eklendi";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["error"] = "Bu tedarikçi adı kullanılıyor!";
                        return View(model);
                    }
                }
                else
                {
                    TempData["error"] = "Bu tedarikçi kodu kullanılıyor!";
                    return View(model);
                }
            }
            else
            {
                TempData["error"] = "Lütfen gerekli bilgilerini girin.";
                return View(model);
            }
        }

        [Authorize(Roles = "WarehouseController")]
        [HttpGet]
        public IActionResult Update(Guid id)
        {
            return View(_supplier.Get(id));
        }

        [Authorize(Roles = "WarehouseController")]
        [HttpPost]
        public async Task<IActionResult> Update(Guid id, Supplier model)
        {
            if(ModelState.IsValid)
            {
                if(!await _supplier.CheckSupplierCodeAsync(id, model.Code))
                {
                    if(!await _supplier.CheckSupplierNameAsync(id, model.Name))
                    {
                        var supplier = _supplier.Get(id);
                        supplier.Code = model.Code;
                        supplier.Name = model.Name;
                        supplier.Address = model.Address;
                        supplier.CreatedAt = model.CreatedAt;
                        supplier.UpdatedAt = DateTime.Now;

                        await _supplier.UpdateAsync(supplier);
                        TempData["success"] = "Tedarikçi kaydı başarılı bir şekilde güncellendi";
                        return RedirectToAction("Update", "Supplier", new { id });

                    }
                    else
                    {
                        TempData["error"] = "Bu tedarikçi adı kullanılıyor!";
                        return View(model);
                    }
                }
                else
                {
                    TempData["error"] = "Bu tedarikçi kodu kullanılıyor!";
                    return View(model);
                }
            }
            else
            {
                TempData["error"] = "Gerekli bilgileri girin";
                return View(model);
            }
        }

        [Authorize(Roles = "WarehouseController")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _supplier.DeleteAsync(id);
            return RedirectToAction("Index", "Supplier");
        }



    }
}

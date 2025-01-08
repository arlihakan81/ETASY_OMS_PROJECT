using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using ETASY_OMS_PROJECT.WebUI.Entity.ViewModels.WarehouseMaterialVM;
using ETASY_OMS_PROJECT.WebUI.Entity.ViewModels.WarehouseVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETASY_OMS_PROJECT.WebUI.Controllers
{
    [Authorize(Roles = "WarehouseClient, WarehouseController")]
    public class WarehouseController : Controller
    {
        private readonly IWarehouseDal _warehouse;

        public WarehouseController(IWarehouseDal warehouse)
        {
            _warehouse = warehouse;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _warehouse.GetAllAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateWarehouseModel model)
        {
            if(ModelState.IsValid)
            {
                var warehouse = new Warehouse
                {
                    Code = model.Warehouse.Code,
                    Description = model.Warehouse.Description,
                    SupplierId = model.Warehouse.SupplierId,
                    CreatedAt = DateTime.Now
                };
                await _warehouse.AddAsync(warehouse);
                TempData["success"] = "Yeni malzeme giriş formu başarılı bir şekilde eklendi";
                return RedirectToAction("Create", "WarehouseMaterial", new { warehouse.Id });
            }
            else
            {
                TempData["error"] = "Gerekli alanları girin";
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Update(Guid id)
        {
            return View(_warehouse.Get(id));
        }

        [HttpPost]
        public async Task<IActionResult> Update(Guid id, UpdateWarehouseModel model)
        {
            if(ModelState.IsValid)
            {
                var warehouse = _warehouse.Get(id);
                warehouse.Code = model.Warehouse.Code;
                warehouse.Description = model.Warehouse.Description;
                warehouse.SupplierId = model.Warehouse.SupplierId;
                warehouse.CreatedAt = model.Warehouse.CreatedAt;
                warehouse.UpdatedAt = DateTime.Now;
                await _warehouse.UpdateAsync(warehouse);
                TempData["success"] = "Malzeme giriş formu başarılı bir şekilde güncellendi";
                return RedirectToAction("Update", "Warehouse", new { id });
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
            await _warehouse.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }


    }
}

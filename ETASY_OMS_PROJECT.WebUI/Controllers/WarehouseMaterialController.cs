using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using ETASY_OMS_PROJECT.WebUI.Entity.ViewModels.WarehouseMaterialVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETASY_OMS_PROJECT.WebUI.Controllers
{
    [Authorize(Roles = "WarehouseController, WarehouseClient")]
    public class WarehouseMaterialController : Controller
    {
        private readonly IWarehouseMaterialDal _warehouse;
        private readonly IWarehouseDal _warehouseDal;

        public WarehouseMaterialController(IWarehouseMaterialDal warehouse, IWarehouseDal warehouseDal)
        {
            _warehouse = warehouse;
            _warehouseDal = warehouseDal;
        }

        [HttpGet]
        public IActionResult Create(Guid id)
        {
            return View(_warehouseDal.Get(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(Guid id, CreateWarehouseMaterialModel model)
        {
            if(ModelState.IsValid)
            {
                var warehouse = new WarehouseMaterial
                {
                    WarehouseId = id,
                    MaterialId = model.WarehouseMaterial.MaterialId,
                    Quantity = model.WarehouseMaterial.Quantity,
                    CreatedAt = DateTime.Now
                };
                await _warehouse.AddAsync(warehouse);
                TempData["error"] = "Depoya yeni bir ürün kaydı başarılı bir şekilde eklendi";
                return RedirectToAction("Create", "WarehouseMaterial", new { id });
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
        public async Task<IActionResult> Update(Guid id, UpdateWarehouseMaterialModel model)
        {
            if (ModelState.IsValid)
            {
                var warehouse = _warehouse.Get(id);
                warehouse.WarehouseId = model.Warehouse.Id;
                warehouse.MaterialId = model.WarehouseMaterial.MaterialId;
                warehouse.Quantity += model.WarehouseMaterial.Quantity;
                warehouse.CreatedAt = model.WarehouseMaterial.CreatedAt;
                warehouse.UpdatedAt = DateTime.Now;
                await _warehouse.UpdateAsync(warehouse);
                TempData["error"] = "Depodaki bir ürün kaydı başarılı bir şekilde eklendi";
                return RedirectToAction("Create", "WarehouseMaterial", new { id });
            }
            else
            {
                TempData["error"] = "Gerekli alanları girin";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _warehouse.DeleteAsync(id);
            return RedirectToAction("Create", "WarehouseMaterial");
        }


    }
}

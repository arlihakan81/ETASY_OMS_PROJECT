using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETASY_OMS_PROJECT.WebUI.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentDal _department;

        public DepartmentController(IDepartmentDal department)
        {
            _department = department;   
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _department.GetAllAsync());
        }

        [Authorize(Roles = "HumanResources")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "HumanResources")]
        [HttpPost]
        public async Task<IActionResult> Create(Department model)
        {
            if(!await _department.CheckDepartmentNameAsync(model.Name))
            {
                await _department.AddAsync(new Department
                {
                    Name = model.Name,
                    CreatedAt = DateTime.Now
                });
                return RedirectToAction("Create");
            }
            else
            {
                TempData["error"] = "Gerekli alanları girin";
                return View(model);
            }
        }

        [Authorize(Roles = "HumanResources")]
        [HttpGet]
        public IActionResult Update(Guid id)
        {
            return View(_department.Get(id));
        }

        [Authorize(Roles = "HumanResources")]
        [HttpPost]
        public async Task<IActionResult> Update(Guid id, Department model)
        {
            if(!await _department.CheckDepartmentNameAsync(id, model.Name))
            {
                var dep = _department.Get(id);
                dep.Name = model.Name;
                dep.CreatedAt = model.CreatedAt;
                dep.UpdatedAt = DateTime.Now;
                await _department.UpdateAsync(dep);
                return RedirectToAction("Update", "Department", new { id });
            }
            else
            {
                TempData["error"] = "Gerekli alanları girin";
                return View(model);
            }
        }

        [Authorize(Roles = "HumanResources")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _department.DeleteAsync(id);
            return RedirectToAction("Index");
        }


    }
}

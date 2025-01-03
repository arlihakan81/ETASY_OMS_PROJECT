﻿using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
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
            await _department.AddAsync(new Department
            {
                Name = model.Name,
                CreatedAt = DateTime.Now
            });
            return RedirectToAction("Create");
        }

        [Authorize(Roles = "HumanResources")]
        [HttpGet]
        public IActionResult Update(int id)
        {
            return View(_department.Get(id));
        }

        [Authorize(Roles = "HumanResources")]
        [HttpPost]
        public async Task<IActionResult> Update(int id, Department model)
        {
            var dep = _department.Get(id);
            dep.Name = model.Name;
            dep.CreatedAt = model.CreatedAt;
            dep.UpdatedAt = DateTime.Now;
            await _department.UpdateAsync(dep);
            return RedirectToAction("Update", "Department", new {id});
        }

        [Authorize(Roles = "HumanResources")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _department.DeleteAsync(id);
            return RedirectToAction("Index");
        }


    }
}

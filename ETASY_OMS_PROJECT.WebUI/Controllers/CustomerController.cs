using AspNetCoreGeneratedDocument;
using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETASY_OMS_PROJECT.WebUI.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ICustomerDal _customer;

        public CustomerController(ICustomerDal customer)
        {
            _customer = customer;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _customer.GetAllAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Customer model)
        {
            if(ModelState.IsValid)
            {
                if(!await _customer.CheckNameAsync(model.Name))
                {
                    await _customer.AddAsync(new Customer
                    {
                        Name = model.Name,
                        Address = model.Address,
                        CreatedAt = DateTime.Now
                    });
                    TempData["success"] = "Yeni müşteri başarılı bir şekilde eklendi";
                    return RedirectToAction(nameof(Create));
                }
                else
                {
                    TempData["error"] = "Bu müşteri adı kullanılıyor";
                    return View(model);
                }
            }
            else
            {
                TempData["error"] = "Müşteri adı ve adres bilgisi boş geçilemez";
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            return View(_customer.Get(id));
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Customer model)
        {
            if(ModelState.IsValid)
            {
                if(!await _customer.CheckNameAsync(id, model.Name))
                {
                    var customer = _customer.Get(id);
                    customer.Name = model.Name;
                    customer.Address = model.Address;
                    customer.CreatedAt = customer.CreatedAt;
                    model.UpdatedAt = DateTime.Now;
                    TempData["success"] = "Müşteri bilgileri başarılı bir şekilde güncellendi";
                    return RedirectToAction("Update", "Customer", new { id });
                }
                else
                {
                    TempData["error"] = "Bu müşteri adı kullanılıyor";
                    return View(model);
                }
            }
            else
            {
                TempData["error"] = "Müşteri adı ve adres bilgisi boş geçilemez";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _customer.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }


    }
}

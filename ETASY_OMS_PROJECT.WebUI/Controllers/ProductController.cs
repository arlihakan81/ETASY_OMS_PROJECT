using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ETASY_OMS_PROJECT.WebUI.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductDal _product;

        public ProductController(IProductDal product)
        {
            _product = product;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _product.GetAllAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product model)
        {
            if(ModelState.IsValid)
            {
                if(await _product.CheckNameAsync(model.Name))
                {
                    TempData["error"] = "Bu ürün adı kullanılıyor";
                    return View(model);
                }
                else
                {
                    if(await _product.CheckCodeAsync(model.Code))
                    {
                        TempData["error"] = "Bu ürün kodu kullanılıyor";
                        return View(model);
                    }
                    else
                    {
                        await _product.AddAsync(new Product
                        {
                            Code = model.Code,
                            Name = model.Name,
                            Scale = model.Scale,
                            CreatedAt = DateTime.Now
                        });
                        TempData["success"] = "Yeni ürün başarılı bir şekilde eklendi";
                        return RedirectToAction("Create", "Product");
                    }
                }
            }
            else
            {
                TempData["error"] = "Ürün eklemek için ürün bilgilerini girin";
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            return View(_product.Get(id));
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Product model)
        {
            if(ModelState.IsValid)
            {
                if(await _product.CheckNameAsync(id, model.Name))
                {
                    TempData["error"] = "Bu ürün adı kullanılıyor";
                    return View(model);
                }
                else
                {
                    if(await _product.CheckCodeAsync(id, model.Code))
                    {
                        TempData["error"] = "Bu ürün kodu kullanılıyor";
                        return View(model);
                    }
                    else
                    {
                        var user = _product.Get(id);
                        user.Code = model.Code;
                        user.Name = model.Name;
                        user.Scale = model.Scale;
                        user.CreatedAt = model.CreatedAt;
                        user.UpdatedAt = DateTime.Now;
                        await _product.UpdateAsync(user);
                        TempData["success"] = "Ürün başarılı bir şekilde güncellendi";
                        return RedirectToAction(nameof(Update));
                    }
                }
            }
            else
            {
                TempData["error"] = "Ürün güncellemek için bilgileri girin.";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _product.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }


    }
}

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
                if(!await _product.CheckNameAsync(model.Name))
                {
                    if (!await _product.CheckCodeAsync(model.Code))
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
                    else
                    {
                        TempData["error"] = "Bu ürün kodu kullanılıyor";
                        return View(model);
                        
                    }
                }
                else
                {
                    TempData["error"] = "Bu ürün adı kullanılıyor";
                    return View(model);
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
                var product = _product.Get(id);
                product.Name = model.Name;
                product.Code = model.Code;
                product.Scale = model.Scale;
                product.CreatedAt = model.CreatedAt;
                product.UpdatedAt = DateTime.Now;
                await _product.UpdateAsync(product);
                TempData["success"] = "Ürün başarılı bir şekilde güncellendi";
                return RedirectToAction(nameof(Update));
            }
            else
            {
                TempData["error"] = "Ürün bilgilerini girin.";
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

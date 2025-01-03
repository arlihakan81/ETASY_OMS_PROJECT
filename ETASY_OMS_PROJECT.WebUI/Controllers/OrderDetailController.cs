using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.Entity.ViewModels.OrderDetailVM;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETASY_OMS_PROJECT.WebUI.Controllers
{
    [Authorize]
    public class OrderDetailController : Controller
    {
        private readonly IOrderDetailDal _detail;
        public OrderDetailController(IOrderDetailDal detail) 
        {
            _detail = detail;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "ExportUser,DomesticUser")]
        [HttpGet]
        public IActionResult Create(int id)
        {
            return View(_detail.GetCreateOrderDetailModel(id));
        }

        [Authorize(Roles = "ExportUser,DomesticUser")]
        [HttpPost]
        public async Task<IActionResult> Create(int id, CreateOrderDetailModel model)
        {
            if(ModelState.IsValid)
            {
                await _detail.AddAsync(new OrderDetail
                {
                    OrderId = id,
                    ProductId = model.OrderDetail.ProductId,
                    Quantity = model.OrderDetail.Quantity,
                    CreatedAt = DateTime.Now
                });
                                                
                TempData["success"] = $"{model.Order.FormId} No'lu Siparişinize yeni bir ürün eklendi.";
                return RedirectToAction("Create", "OrderDetail", new { model.OrderDetail.OrderId });
            }
            else
            {
                TempData["error"] = "Sipariş boş geçilemez!";
                return View(model);
            }
        }

        [Authorize(Roles = "ExportUser,DomesticUser")]
        [HttpGet]
        public IActionResult Update(int id)
        {
            return View(_detail.GetUpdateOrderDetailModel(id));
        }

        [Authorize(Roles = "ExportUser,DomesticUser")]
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateOrderDetailModel model)
        {
            if (ModelState.IsValid)
            {
                await _detail.AddAsync(new OrderDetail
                {
                    OrderId = id,
                    ProductId = model.OrderDetail.ProductId,
                    Quantity = model.OrderDetail.Quantity,
                    CreatedAt = DateTime.Now
                });

                TempData["success"] = $"{model.Order.FormId} No'lu Siparişinize yeni bir ürün eklendi.";
                return RedirectToAction("Create", "OrderDetail", new { model.OrderDetail.OrderId });
            }
            else
            {
                TempData["error"] = "Sipariş boş geçilemez!";
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            return View(_detail.GetDetailOrderDetailModel(id));
        }

    }
}

using System.Security.Claims;
using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using ETASY_OMS_PROJECT.WebUI.Entity.ViewModels.OrderVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETASY_OMS_PROJECT.WebUI.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderDal _order;

        public OrderController(IOrderDal order)
        {
            _order = order;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _order.GetAllAsync());
        }

        [Authorize(Roles = "ExportUser,DomesticUser,Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View(_order.GetCreateOrderModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderModel model)
        {
            if (ModelState.IsValid)
            {
                if(!await _order.CheckFormIdAsync(model.Order.FormId))
                {
                    await _order.AddAsync(new Order
                    {
                        FormId = model.Order.FormId,
                        CustomerId = model.Order.CustomerId,
                        DueDate = model.Order.DueDate,
                        Status = model.Order.Status,
                        DepartmentId = int.Parse(User.FindFirstValue("DepartmentId")),
                        CreatedAt = DateTime.Now
                    });
                    TempData["success"] = "Yeni sipariş başarılı bir şekilde eklendi";
                    return RedirectToAction(nameof(Create));
                }
                else
                {
                    TempData["error"] = "Bu form numarası kullanılıyor";
                    return View(model);
                }
            }
            else
            {
                TempData["error"] = "Sipariş bilgilerini girin";
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            return View(_order.GetUpdateOrderModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateOrderModel model)
        {
            if (ModelState.IsValid)
            {
                var ord = _order.Get(id);
                ord.FormId = model.Order.FormId;
                ord.CustomerId = model.Order.CustomerId;
                ord.DueDate = model.Order.DueDate;
                ord.Status = model.Order.Status;
                ord.DepartmentId = int.Parse(User.FindFirstValue("DepartmentId"));
                ord.CreatedAt = model.Order.CreatedAt;
                ord.UpdatedAt = DateTime.Now;
                await _order.UpdateAsync(ord);
                TempData["success"] = "Sipariş başarılı bir şekilde güncellendi";
                return RedirectToAction(nameof(Update));
            }
            else
            {
                TempData["error"] = "Sipariş bilgilerini girin";
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            return View(_order.GetDetailOrderModel(id));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _order.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }


    }
}

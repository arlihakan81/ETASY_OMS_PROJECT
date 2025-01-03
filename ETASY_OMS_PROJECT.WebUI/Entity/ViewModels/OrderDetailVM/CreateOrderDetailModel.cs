using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ETASY_OMS_PROJECT.WebUI.Entity.ViewModels.OrderDetailVM
{
    public class CreateOrderDetailModel
    {
        public Order Order { get; set; }
        public List<SelectListItem> Products { get; set; }
        public OrderDetail OrderDetail { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}

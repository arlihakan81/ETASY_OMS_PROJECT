using ETASY_OMS_PROJECT.WebUI.Entity.Entities;

namespace ETASY_OMS_PROJECT.WebUI.Entity.ViewModels.OrderDetailVM
{
    public class OrderDetailsModel
    {
        public List<Order> Orders { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}

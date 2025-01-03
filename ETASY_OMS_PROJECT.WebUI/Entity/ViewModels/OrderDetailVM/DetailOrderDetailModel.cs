using ETASY_OMS_PROJECT.WebUI.Entity.Entities;

namespace ETASY_OMS_PROJECT.WebUI.Entity.ViewModels.OrderDetailVM
{
    public class DetailOrderDetailModel
    {
        public List<OrderDetail> OrderDetails { get; set; }
        public Order Order { get; set; }
        public OrderDetail OrderDetail { get; set; }
    }
}

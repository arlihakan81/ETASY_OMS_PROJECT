using ETASY_OMS_PROJECT.WebUI.Entity.Entities;

namespace ETASY_OMS_PROJECT.WebUI.Entity.ViewModels.OrderVM
{
    public class DetailOrderModel
    {
        public List<OrderDetail> OrderDetails { get; set; }
        public Order Order { get; set; }
        public List<Product> Products { get; set; }


    }
}

using ETASY_OMS_PROJECT.WebUI.Entity.Entities;

namespace ETASY_OMS_PROJECT.WebUI.Entity.ViewModels.OrderVM
{
    public class CreateOrderModel
    {
        public Order Order { get; set; }
        public List<Customer> Customers { get; set; }
    }
}

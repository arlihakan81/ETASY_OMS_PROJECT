using ETASY_OMS_PROJECT.WebUI.Entity.Entities.Concrete;

namespace ETASY_OMS_PROJECT.WebUI.Entity.Entities
{
    public class OrderDetail : EntityBase
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }

    }
}

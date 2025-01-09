using ETASY_OMS_PROJECT.WebUI.Entity.Entities.Concrete;
using ETASY_OMS_PROJECT.WebUI.Entity.Enums.Order;

namespace ETASY_OMS_PROJECT.WebUI.Entity.Entities
{
    public class Order : EntityBase
    {
        public string FormId { get; set; }
        public string Description { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public Status Status { get; set; }
        public DateOnly DueDate { get; set; }
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
        public OrderDetail OrderDetail { get; set; }
    }
}

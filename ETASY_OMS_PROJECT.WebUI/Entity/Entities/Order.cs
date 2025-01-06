using ETASY_OMS_PROJECT.WebUI.Entity.Entities.Concrete;
using ETASY_OMS_PROJECT.WebUI.Entity.Enums.Order;

namespace ETASY_OMS_PROJECT.WebUI.Entity.Entities
{
    public class Order : EntityBase
    {
        public int FormId { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public Status Status { get; set; }
        public DateOnly DueDate { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public OrderDetail OrderDetail { get; set; }
    }
}

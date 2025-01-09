using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using ETASY_OMS_PROJECT.WebUI.Entity.ViewModels.OrderVM;

namespace ETASY_OMS_PROJECT.WebUI.Entity.ViewModels
{
    public class IndexViewModel
    {
        public List<Order> Orders { get; set; }
        public List<Customer> Customers { get; set; }
        public List<User> Users { get; set; }
        public List<Product> Products { get; set; }
        public List<Department> Departments { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public Export Export { get; set; }
        public Domestic Domestic { get; set; }
        public List<Material> Materials { get; set; }
        public List<Warehouse> Warehouses { get; set; }
        public List<Supplier> Suppliers { get; set; }
        public List<Group> Groups { get; set; }
    }
}

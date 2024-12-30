using ETASY_OMS_PROJECT.WebUI.Entity.Entities;

namespace ETASY_OMS_PROJECT.WebUI.Entity.ViewModels
{
    public class IndexViewModel
    {
        public List<Order> Orders { get; set; }
        public List<Customer> Customers { get; set; }
        public List<User> Users { get; set; }
        public List<Product> Products { get; set; }
        public List<Department> Departments { get; set; }
    }
}

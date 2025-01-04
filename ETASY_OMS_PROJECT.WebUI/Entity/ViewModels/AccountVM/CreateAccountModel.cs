using ETASY_OMS_PROJECT.WebUI.Entity.Entities;

namespace ETASY_OMS_PROJECT.WebUI.Entity.ViewModels.AccountVM
{
    public class CreateAccountModel
    {
        public List<Department> Departments { get; set; }
        public User User { get; set; }
    }
}

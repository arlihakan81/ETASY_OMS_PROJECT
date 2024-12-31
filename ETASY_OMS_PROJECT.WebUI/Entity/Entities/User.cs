using System.ComponentModel.DataAnnotations.Schema;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities.Concrete;
using ETASY_OMS_PROJECT.WebUI.Entity.Enums.User;

namespace ETASY_OMS_PROJECT.WebUI.Entity.Entities
{
    public class User : EntityBase
    {
        public string Avatar { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }
    }
}

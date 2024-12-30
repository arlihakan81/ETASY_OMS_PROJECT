using System.ComponentModel.DataAnnotations;

namespace ETASY_OMS_PROJECT.WebUI.Entity.ViewModels
{
    public class RegisterViewModel : LoginViewModel
    {
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}

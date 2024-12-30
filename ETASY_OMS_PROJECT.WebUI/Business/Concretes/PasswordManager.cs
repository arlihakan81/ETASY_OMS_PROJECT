using ETASY_OMS_PROJECT.WebUI.Business.Abstracts;
using NETCore.Encrypt.Extensions;

namespace ETASY_OMS_PROJECT.WebUI.Business.Concretes
{
    public class PasswordManager : IPasswordService
    {
        public string HashPassword(string password)
        {
            var salt = "asdkmasdaskdmasdkasmd";
            var salted = salt + password;
            var hashed = salted.MD5();
            return hashed;
        }
    }
}

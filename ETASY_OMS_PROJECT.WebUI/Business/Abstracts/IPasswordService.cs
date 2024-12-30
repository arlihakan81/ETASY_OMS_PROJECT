namespace ETASY_OMS_PROJECT.WebUI.Business.Abstracts
{
    public interface IPasswordService
    {
        string HashPassword(string password);
    }
}

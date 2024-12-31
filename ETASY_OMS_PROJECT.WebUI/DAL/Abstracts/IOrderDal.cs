using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Abstract;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using ETASY_OMS_PROJECT.WebUI.Entity.ViewModels.OrderVM;

namespace ETASY_OMS_PROJECT.WebUI.DAL.Abstracts
{
    public interface IOrderDal : IGenericRepository<Order>
    {
        Task<bool> CheckFormIdAsync(int formId);
        Task<bool> CheckFormIdAsync(int id, int formId);
        CreateOrderModel GetCreateOrderModel();
        UpdateOrderModel GetUpdateOrderModel(int id);
        DetailOrderModel GetDetailOrderModel(int id);
    }
}

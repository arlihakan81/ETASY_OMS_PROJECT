using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Abstract;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using ETASY_OMS_PROJECT.WebUI.Entity.ViewModels.OrderVM;

namespace ETASY_OMS_PROJECT.WebUI.DAL.Abstracts
{
    public interface IOrderDal : IGenericRepository<Order>
    {
        Task<bool> CheckFormIdAsync(string formId);
        Task<bool> CheckFormIdAsync(Guid id, string formId);
        CreateOrderModel GetCreateOrderModel();
        UpdateOrderModel GetUpdateOrderModel(Guid id);
        DetailOrderModel GetDetailOrderModel(Guid id);
    }
}

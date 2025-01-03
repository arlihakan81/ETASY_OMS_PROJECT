using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Abstract;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using ETASY_OMS_PROJECT.WebUI.Entity.ViewModels.OrderDetailVM;

namespace ETASY_OMS_PROJECT.WebUI.DAL.Abstracts
{
    public interface IOrderDetailDal : IGenericRepository<OrderDetail>
    {
        CreateOrderDetailModel GetCreateOrderDetailModel(int orderId);
        UpdateOrderDetailModel GetUpdateOrderDetailModel(int id);
        DetailOrderDetailModel GetDetailOrderDetailModel(int id);
    }
}

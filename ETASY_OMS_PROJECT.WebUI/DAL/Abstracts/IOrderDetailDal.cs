using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Abstract;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities;
using ETASY_OMS_PROJECT.WebUI.Entity.ViewModels.OrderDetailVM;

namespace ETASY_OMS_PROJECT.WebUI.DAL.Abstracts
{
    public interface IOrderDetailDal : IGenericRepository<OrderDetail>
    {
        CreateOrderDetailModel GetCreateOrderDetailModel(Guid id);
        UpdateOrderDetailModel GetUpdateOrderDetailModel(Guid id);
        DetailOrderDetailModel GetDetailOrderDetailModel(Guid id);
        OrderDetailsModel GetOrderDetailsModel();
        OrderDetail GetByOrderId(Guid id);
    }
}

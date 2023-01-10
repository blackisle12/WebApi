using WebApi.Models.Order;
using static WebApi.Common.Enums;

namespace WebApi.Services.Interface
{
    public interface IOrderService
    {
        Task<int> CreateAsync(CreateOrderModel model);
        Task EditAsync(EditOrderModel model);
        Task<List<GetOrderModel>> GetAllAsync(int? id = null, OrderStatus? status = null);
    }
}

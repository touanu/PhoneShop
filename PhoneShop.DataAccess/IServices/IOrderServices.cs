using PhoneShop.DataAccess.DTO;

namespace PhoneShop.DataAccess.IServices
{
    public interface IOrderServices
    {
        Task<ReturnData> InsertOrder(OrderRequestData requestData);
        Task<ReturnData> DeleteOrder(int orderId);
        Task<ReturnData> UpdateOrder(OrderRequestData requestData);
        Task<OrderGetByIdReturnData> GetOrderById(int id);
        Task<List<Order>> GetOrders();
    }
}

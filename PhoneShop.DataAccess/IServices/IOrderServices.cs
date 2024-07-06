using PhoneShop.DataAccess.DTO;

namespace PhoneShop.DataAccess.IServices
{
    public interface IOrderServices
    {
        Task<ReturnData> Order_Add(OrderRequestData requestData);
        Task<ReturnData> Order_Delete(string orderId);
        Task<ReturnData> Order_Update(OrderRequestData requestData);
    }
}
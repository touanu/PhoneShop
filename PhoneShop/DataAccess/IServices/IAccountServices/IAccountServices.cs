using PhoneShop.DataAccess.DTO;
using System.ComponentModel.DataAnnotations;

namespace PhoneShop.DataAccess.IServices.IAccountServices
{
    public interface IAccountServices
    {
        Task<ReturnData> LogIn(LoginRequestData RequestData);
        Task<ReturnData> LogOut(LoginRequestData RequestData);
        Task<ReturnData> SingInCustomer(LoginRequestData RequestData);
    }
}

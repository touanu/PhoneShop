using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhoneShop.DataAccess.DTO;
using PhoneShop.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PhoneShop.DataAccess.IServices
{
    public interface IAccountServices
    {
        Task<ReturnDataReturnAccount> AccountLogin(AccountRequestData requestData);
        Task<ReturnDataReturnAccount> AddCustomer(AccountRequestData requestData);
        Task<ReturnData> RemoveCustomerByID(AccountRequestData requestData);
        Task<List<Customer>> GetCustomerByUserNameorLastName(AccountRequestData requestData);
        Task<ReturnData> UpdateCustomer(AccountRequestData requestData);
        Task<Functions> GetFunction(string functionCode);
        Task<User_Permission> User_PermissionById(int functionId, int UserID);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhoneShop.DataAccess.DTO;
using PhoneShop.Models;

namespace PhoneShop.DataAccess.IServices
{
    public interface IAccountServices
    {
        Task<ReturnData> AccountLogin(AccountRequestData requestData);
        Task<ReturnData> SingInCustomer(AccountRequestData requestData);
        Task<ReturnData> AddCustomer(AccountRequestData requestData);
        Task<ReturnData> RemoveCustomerByID(AccountRequestData requestData);
        Task<List<Customers>> GetCustomerByUserNameorLastName(AccountRequestData requestData);
        Task<ReturnData> UpdateCustomer(AccountRequestData requestData);
    }
}
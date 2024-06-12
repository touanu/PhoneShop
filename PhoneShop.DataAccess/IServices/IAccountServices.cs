using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhoneShop.DataAccess.DTO;

namespace PhoneShop.DataAccess.IServices
{
    public interface IAccountServices
    {
        Task<ReturnData> AccountLogin(AccountRequestData requestData);
    }
}
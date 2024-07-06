using PhoneShop.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.IServices
{
    public interface IOrderServices
    {
        public Task<ReturnData> Order_Add(OrderRequestData requestData);
        public Task<ReturnData> Order_Delete(string orderId);
        public Task<ReturnData> Order_Update(OrderRequestData requestData);

    }
}

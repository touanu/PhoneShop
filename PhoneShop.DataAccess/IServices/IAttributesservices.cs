using PhoneShop.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.IServices
{
    public interface IAttributesservices
    {
        Task<List<ProductAttribute>> GetAttributes(AttributesResponseData responseData);
        Task<ReturnData> AddAttributes(AttributesRequestData requestData);
        Task<ReturnData> UpdateAttributes(AttributesRequestData requestData);
        Task<ReturnData> DeleteAttributes(AttributesRequestData requestData);
        Task<ReturnData> DeleteAtributesVallue(AttributesRequestData requestData);
    }
}

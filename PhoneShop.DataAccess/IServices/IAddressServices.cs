using PhoneShop.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.IServices
{
    public interface IAddressServices
    {
        Task<AddressReturnData> GetAddressByWardID(int wardID);
        Task<List<Province>?> GetProvinces();
        Task<List<District>?> GetDistricts(int provinceID);
        Task<List<Ward>?> GetWards(int districtID);
    }
}

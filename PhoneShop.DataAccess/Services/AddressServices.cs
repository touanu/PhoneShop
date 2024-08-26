using Microsoft.EntityFrameworkCore;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.Services
{
    public class AddressServices(PhoneShopDBcontext phoneShopDBcontext) : IAddressServices
    {
        private readonly PhoneShopDBcontext _phoneShopDBContext = phoneShopDBcontext;

        public async Task<AddressReturnData> GetAddressByWardID(int wardID)
        {
            var returnData = new AddressReturnData();

            try
            {
                var ward = await _phoneShopDBContext.Ward.FindAsync(wardID);
                if (ward == null)
                {
                    returnData.ReturnCode = (int)ReturnCode.NotFound;
                    returnData.ReturnMsg = "Không tìm thấy phường/xã này.";
                    return returnData;
                }

                var district = await _phoneShopDBContext.District.FindAsync(ward.DistrictID);
                if (district == null)
                {
                    returnData.ReturnCode = (int)ReturnCode.NotFound;
                    returnData.ReturnMsg = "Không tìm thấy huyện này.";
                    return returnData;
                }

                var province = await _phoneShopDBContext.Province.FindAsync(district.ProvinceID);
                if (province == null)
                {
                    returnData.ReturnCode = (int)ReturnCode.NotFound;
                    returnData.ReturnMsg = "Không tìm thấy tỉnh/thành phố này.";
                    return returnData;
                }

                returnData.Ward = ward.FullName;
                returnData.Province = province.FullName;
                returnData.District = district.FullName;

                returnData.ReturnCode = (int)ReturnCode.Success;
                returnData.ReturnMsg = "Lấy thông tin địa chỉ thành công.";
                return returnData;
            }
            catch (Exception ex)
            {
                returnData.ReturnCode = (int)ReturnCode.Exception;
                returnData.ReturnMsg = ex.Message;
                return returnData;
            }
        }

        public async Task<List<District>?> GetDistricts(int provinceID)
        {
            return await _phoneShopDBContext.District.Where(
                        x => x.DistrictID == provinceID
                    ).ToListAsync();
        }

        public async Task<List<Province>?> GetProvinces()
        {
            return await _phoneShopDBContext.Province.ToListAsync();
        }

        public async Task<List<Ward>?> GetWards(int districtID)
        {
            return await _phoneShopDBContext.Ward.Where(
                    x => x.DistrictID == districtID
                ).ToListAsync();
        }
    }
}

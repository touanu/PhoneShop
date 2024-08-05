using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhoneShop.Commonlibs;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.IServices;

namespace PhoneShop.DataAccess.Services
{
    public class BrandServices : IBrandServices
    {
        PhoneShopDBcontext dbcontext;
        public BrandServices(PhoneShopDBcontext _dbcontext)
        {
            dbcontext = _dbcontext;
        }

        public async Task<BrandListReturnData> BrandsGetList(BrandRequetsData requestData)
        {
            var list = new List<Brand>();
            var returnData = new BrandListReturnData();
            try
            {
                list = dbcontext.Brand.ToList();

                if (!string.IsNullOrEmpty(requestData.BrandName))
                {
                    list = list.FindAll(s => s.BrandName.ToLower().Contains(requestData.BrandName.ToLower())).ToList();
                }
                returnData.ReturnCode = 1;
                returnData.ReturnMsg = "lấy dữ liệu thành công!";
                returnData.list = list;
                return returnData;
            }
            catch (Exception ex)
            {
                returnData.ReturnCode = -969;
                returnData.ReturnMsg = "Hệ thống đang bận!" + ex;
                return returnData;
            }
        }

        public async Task<BrandInsertReturnData> BrandInsertUpdate(BrandInsertRequestData requestData)
        {
            var returnData = new BrandInsertReturnData();
            var errItem = string.Empty;
            try
            {
                if (requestData == null
                    || string.IsNullOrEmpty(requestData.BrandName)
                    )

                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "Dữu liệu không hợp lệ";
                    return returnData;
                }


                // thêm sản phẩm vào database 

                var BrandReq = new Brand
                {
                    BrandName = requestData.BrandName,
              
                    IconImages = requestData.IconImages,

                };

                dbcontext.Brand.Add(BrandReq);
                dbcontext.SaveChanges();
                returnData.ReturnCode = 1;
                returnData.ReturnMsg = "Thêm thương hiệu thành công";
                return returnData;
            }

            catch (Exception ex)
            {

                returnData.ReturnCode = -969;
                returnData.ReturnMsg = "Hệ thống đang bận!";
                return returnData;
            }
   
        }
        public async Task<List<Brand>> BrandGetAll()
        {
            var returnData = new List<Brand>();
            try
            {
                returnData = dbcontext.Brand.ToList();
                return( returnData );
            }

            catch (Exception ex)
            {

                throw ex;
            }
        }
    

        public async Task<Brand_DeleteReturnData> Brand_Delete(Brand_DeleteRequestData requestData)
        {
            var returnData = new Brand_DeleteReturnData();
            try
            {
                // cần kiểm tra xem id muốn xóa có tồn tại không
                var Brand = dbcontext.Brand.Where(s => s.BrandID == requestData.BrandID).FirstOrDefault();

                if (Brand == null || Brand?.BrandID <= 0)
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "Thương hiệu cần xóa không có trên hệ thống";
                    return returnData;
                }


                dbcontext.Brand.Remove(Brand);

                returnData.ReturnCode = 1;
                returnData.ReturnMsg = "Xóa thương hiệu thành công";
                return returnData;
            }
            catch (Exception ex)
            {
                returnData.ReturnCode = -969;
                returnData.ReturnMsg = "Hệ thống đang bận!";
                return returnData;
            }
        }

       
    }
}
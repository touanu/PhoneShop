using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhoneShop.Commonlibs;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.IServices;
using PhoneShop.Models;

namespace PhoneShop.DataAccess.Services
{
    public class BrandServices : IBrandServices
    {
        PhoneShopDBcontext dbcontext;
        public BrandServices(PhoneShopDBcontext _dbcontext)
        {
            dbcontext = _dbcontext;
        }

        public async Task<List<Brands>> BrandsGetList()
        {
            var list = new List<Brands>();
            try
            {
                var listBrand = dbcontext.brands.ToList();
                foreach (var item in listBrand)
                {
                    
                    var brand_attb = dbcontext.brands.ToList().FindAll(x => x.BrandID == item.BrandID);

                    var brand = new Brands();

                    brand.BrandID = item.BrandID;
                    brand.BrandName = item.BrandName;

                    list.Add(brand);

                }

                return list;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<BrandInsertReturnData> BrandInsertUpdate(BrandInsertRequestData requestData)
        {
            var returnData = new BrandInsertReturnData();
            var errItem = string.Empty;
            try
            {
                if (requestData == null
                    || requestData.BrandID == 0
                    || string.IsNullOrEmpty(requestData.BrandName)
                    )

                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "Dữu liệu không hợp lệ";
                    return returnData;
                }


                // thêm sản phẩm vào database 

                var BrandReq = new Brands
                {
                    BrandName = requestData.BrandName,
                    IconImages = requestData.IconImages,

                };

                dbcontext.brands.Add(BrandReq);

                dbcontext.SaveChangesAsync();


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

    

        public async Task<Brand_DeleteReturnData> Brand_Delete(Brand_DeleteRequestData requestData)
        {
            var returnData = new Brand_DeleteReturnData();
            try
            {
                // cần kiểm tra xem id muốn xóa có tồn tại không
                var Brand = dbcontext.brands.Where(s => s.BrandID == requestData.BrandID).FirstOrDefault();

                if (Brand == null || Brand?.BrandID <= 0)
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "Thương hiệu cần xóa không có trên hệ thống";
                    return returnData;
                }


                dbcontext.brands.Remove(Brand);
                dbcontext.SaveChangesAsync();

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
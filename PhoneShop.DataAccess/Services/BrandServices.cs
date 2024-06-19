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
    public class BrandServices
    {
        PhonShopDBcontext dbcontext;
        public BrandServices(PhonShopDBcontext _dbcontext)
        {
            dbcontext = _dbcontext;
        }

        public async Task<List<Brands>> BrandGetList()
        {
            var list = new List<Brands>();
            try
            {
                var listProduct = dbcontext.brands.ToList();
                foreach (var item in listProduct)
                {
                    // lấy attribute theo productId 
                    var p_attr = dbcontext.brands.ToList().FindAll(x => x.BrandID == item.BrandID);

                    var product = new Brands();

                    product.BrandID = item.BrandID;
                    product.BrandName = item.BrandName;

                    list.Add(product);

                }

                return list;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<BrandInsertReturnData> BrandInsertUpdate(BrandRequetsData requestData)
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
            }

            catch (Exception ex) { }
            return returnData;
        }

    }
}
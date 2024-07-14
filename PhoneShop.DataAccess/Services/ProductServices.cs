using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.Services
{
    public class ProductServices : IProductServices
    {
        private readonly PhoneShopDBcontext _phoneShopDBContext;
        public async Task<GetProductReturnData> GetProductById(int id)
        {
            var returnData = new GetProductReturnData();

            try
            {
                var product = _phoneShopDBContext.Product.Find(id);

                if (product == null)
                {
                    returnData.ReturnCode = (int)ReturnCode.EqualNull;
                    returnData.ReturnMsg = $"Sản phẩm với id {id} không tồn tại.";
                    return returnData;
                }

                returnData.ReturnCode = (int)ReturnCode.Success;
                returnData.ReturnMsg = $"Tìm thấy sản phẩm với id {id}.";
                returnData.Product = product;
                return returnData;
            }
            catch (Exception ex)
            {
                returnData.ReturnCode = (int)ReturnCode.Exception;
                returnData.ReturnMsg = ex.Message;
                return returnData;
            }
        }

        public async Task<List<Product>> GetProducts()
        {
            return _phoneShopDBContext.Product.ToList();
        }
    }
}

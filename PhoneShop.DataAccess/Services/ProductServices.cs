using Microsoft.EntityFrameworkCore;
using PhoneShop.Commonlibs;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.IServices;

namespace PhoneShop.DataAccess.Services
{
    public class ProductServices(PhoneShopDBcontext phoneShopDBcontext) : IProductServices
    {
        private readonly PhoneShopDBcontext _phoneShopDBContext = phoneShopDBcontext;
        public async Task<GetProductReturnData> GetProductById(int id)
        {
            var returnData = new GetProductReturnData();

            try
            {
                var product = await _phoneShopDBContext.Product.FindAsync(id);

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
            return await _phoneShopDBContext.Product.ToListAsync();
        }
        public async Task<ReturnData> InsertProduct(ProductRequestData requestData)
        {
            var returnData = new ReturnData();
            try
            {
                if (requestData == null
                    || !requestData.Product.ProductName.IsName()
                    || !requestData.Product.ProductDescription.IsContainHTMLTags()
                    )
                {
                    returnData.ReturnCode = (int)ReturnCode.Invalid;
                    returnData.ReturnMsg = "Dữ liệu về sản phẩm không hợp lệ.";
                    return returnData;
                }

                if (requestData.Attributes.Exists(x => x.AttributesName.IsName()))
                {
                    returnData.ReturnCode = (int)ReturnCode.Invalid;
                    returnData.ReturnMsg = "Dữ liệu về thuộc tính không hợp lệ";
                    return returnData;
                }

                if (!await _phoneShopDBContext.Brand.AnyAsync(x => x.BrandID == requestData.Product.BrandID))
                {
                    returnData.ReturnCode = (int)ReturnCode.NotExist;
                    returnData.ReturnMsg = "Nhãn hàng này không tồn tại!";
                    return returnData;
                }

                if (!await _phoneShopDBContext.Category.AnyAsync(x => x.CategoryID == requestData.Product.CategoryID))
                {
                    returnData.ReturnCode = (int)ReturnCode.NotExist;
                    returnData.ReturnMsg = "Danh mục này không tồn tại!";
                    return returnData;
                }

                await _phoneShopDBContext.Product.AddAsync(requestData.Product);
                await _phoneShopDBContext.ProductAttribute.AddRangeAsync(requestData.Attributes);
                await _phoneShopDBContext.ProductAttributeValue.AddRangeAsync(requestData.AttributeValues);

                var result = await _phoneShopDBContext.SaveChangesAsync();

                returnData.ReturnCode = (int)ReturnCode.Success;
                returnData.ReturnMsg = "Thêm dữ liệu thành công.";
                return returnData;
            }
            catch (Exception ex)
            {
                returnData.ReturnCode = (int)ReturnCode.Exception;
                returnData.ReturnMsg = ex.Message;
                return returnData;
            }
        }
    }
}

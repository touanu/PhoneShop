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
            // Tương đương với .ToList()
            return [.. _phoneShopDBContext.Product];
        }
        public async Task<ProductGetReturnData> GetProducts(ProductRequestGetData requestData)
        {
            var itemsToSkip = (requestData.PageNumber - 1) * requestData.PageSize;
            var items = _phoneShopDBContext.Product
                .Skip(itemsToSkip)
                .Take(requestData.PageSize);
            var maxPageCount = _phoneShopDBContext.Product.Count() / requestData.PageSize;

            var returnData = new ProductGetReturnData
            {
                Products = [.. items],
                CurrentPage = requestData.PageNumber,
                MaxPageCount = maxPageCount
            };

            return returnData;
        }
        public async Task<ReturnData> InsertProduct(ProductRequestData requestData)
        {
            var returnData = new ReturnData();
            try
            {
                _phoneShopDBContext.Product.Add(requestData.Product);
                _phoneShopDBContext.ProductAttribute.AddRange(requestData.Attributes);
                _phoneShopDBContext.ProductAttributeValue.AddRange(requestData.AttributeValues);

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

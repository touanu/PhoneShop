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
        public async Task<ReturnData> InsertProduct(ProductRequestAddUpdateData requestData)
        {
            var returnData = new ProductAddReturnData();
            try
            {
                if (requestData == null
                    || !requestData.ProductName.IsValidated()
                    || !requestData.ProductDescription.IsValidated()
                    || requestData.BrandID == null
                    || requestData.CategoryID == null
                    || !requestData.Images.IsValidated()
                    || !requestData.Attributes.IsValidated()
                    )
                {
                    returnData.ReturnCode = (int)ReturnCode.Invalid;
                    returnData.ReturnMsg = "Dữ liệu về sản phẩm không hợp lệ.";
                    return returnData;
                }

                // Kiểm tra trùng
                var product = _phoneShopDBContext.Product.Where(s =>
                    s.ProductName == requestData.ProductName).FirstOrDefault();
                if (product != null || product?.ProductID > 0)
                {
                    returnData.ReturnCode = (int)ReturnCode.AlreadyExist;
                    returnData.ReturnMsg = "Tên sản phẩm này đã tồn tại";
                    return returnData;
                }

                // Thêm sản phẩm vào database
                var productRequest = new Product
                {
                    ProductName = requestData.ProductName,
                    BrandID = requestData.BrandID,
                    CategoryID = requestData.CategoryID,
                    ProductDescription = requestData.ProductDescription,
                    Images = requestData.Images,
                    CreatedDate = DateTime.Now,
                };

                // SaveChanges được thực hiện ở đây để lấy ra được ID
                // https://stackoverflow.com/a/5212787
                await _phoneShopDBContext.Product.AddAsync(productRequest);
                await _phoneShopDBContext.SaveChangesAsync();

                // Lưu Attribute
                // Reference về ví dụ
                // Dung lượng_128GB,10,1900000,1800000|Màu_Đen,12,10000000,9900000_Trắng,9,10000000,9900000
                var attributes = requestData.Attributes.Split('|');
                var attributeCount = attributes.Length;

                for (int i = 0; i < attributeCount; i++)
                {
                    var attributeDetails = attributes[i].Split("_");
                    var attributeName = attributeDetails[0];
                    var attributeValues = attributeDetails.Skip(1).ToList();

                    var AttributeRequest = new ProductAttribute
                    {
                        AttributesName = attributeName,
                        ProductID = productRequest.ProductID,
                    };

                    await _phoneShopDBContext.ProductAttribute.AddAsync(AttributeRequest);
                    await _phoneShopDBContext.SaveChangesAsync();

                    var response = await AddAttributeValue(AttributeRequest, attributeValues);
                    if (response.ReturnCode < 0)
                    {
                        _phoneShopDBContext.Product.Remove(productRequest);
                        returnData.ReturnCode = response.ReturnCode;
                        returnData.ReturnMsg = response.ReturnMsg;
                        return returnData;
                    }
                }

                await _phoneShopDBContext.SaveChangesAsync();
                returnData.ReturnCode = (int)ReturnCode.Success;
                returnData.ReturnMsg = "Thêm sản phẩm thành công";
                return returnData;
            }
            catch (Exception ex)
            {
                returnData.ReturnCode = (int)ReturnCode.Exception;
                returnData.ReturnMsg = ex.Message;
                return returnData;
            }
        }

        private async Task<ReturnData> AddAttributeValue(ProductAttribute attribute, List<string> attributeValues)
        {
            var returnData = new ProductAddReturnData();
            try
            {
                for (int i = 0; i < attributeValues.Count; i++)
                {
                    var values = attributeValues[i].Split(",");
                    var valueName = values[0];
                    var valueQuantity = values[1];
                    var valuePrice = values[2];
                    var valuePriceSale = values[3];

                    var AttributeValueRequest = new ProductAttributeValue
                    {
                        AttributeValuesName = valueName,
                        ProductAttributeID = attribute.ProductAttributeID,
                        Price = Convert.ToInt32(valuePrice),
                        PriceSale = Convert.ToInt32(valuePriceSale),
                        Quantity = Convert.ToInt32(valueQuantity),
                    };

                    await _phoneShopDBContext.ProductAttributeValue.AddAsync(AttributeValueRequest);
                }

                await _phoneShopDBContext.SaveChangesAsync();
                returnData.ReturnCode = (int)ReturnCode.Success;
                returnData.ReturnMsg = "Thêm dữ liệu thành công";
                return returnData;
            }
            catch (Exception ex)
            {
                _phoneShopDBContext.ProductAttribute.Remove(attribute);
                returnData.ReturnCode = (int)ReturnCode.Exception;
                returnData.ReturnMsg = ex.Message;
                return returnData;
            }
        }
    }
}

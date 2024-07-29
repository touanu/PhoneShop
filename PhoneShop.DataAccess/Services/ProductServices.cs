using Microsoft.EntityFrameworkCore;
using PhoneShop.Commonlibs;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.IServices;

namespace PhoneShop.DataAccess.Services
{
    public class ProductServices(PhoneShopDBcontext phoneShopDBcontext) : IProductServices
    {
        private readonly PhoneShopDBcontext _phoneShopDBContext = phoneShopDBcontext;

        public async Task<ReturnData> DeleteProduct(ProductRequestDeleteData requestData)
        {
            var returnData = new ReturnData();

            try
            {
                if (requestData == null
                    || requestData.ProductID == null
                    || requestData.ProductID < 0
                    )
                {
                    returnData.ReturnCode = (int)ReturnCode.Invalid;
                    returnData.ReturnMsg = "Dữ liệu về sản phẩm không hợp lệ.";
                    return returnData;
                }

                var promotions = await _phoneShopDBContext.Promotion.Where(
                        x => x.ProductID == requestData.ProductID
                    ).ToListAsync();
                var product = await _phoneShopDBContext.Product
                    .FindAsync(requestData.ProductID);

                if (product == null)
                {
                    returnData.ReturnCode = (int)ReturnCode.NotExist;
                    returnData.ReturnMsg = "Sản phẩm yêu cầu không tồn tại.";
                    return returnData;
                }

                var productAttributes = await _phoneShopDBContext.ProductAttribute.Where(
                    x => x.ProductID == requestData.ProductID).ToListAsync();

                List<ProductAttributeValue> attributeValues = [];
                foreach (var attribute in productAttributes)
                {
                    var values = await _phoneShopDBContext.ProductAttributeValue
                        .Where(x => x.ProductAttributeID == attribute.ProductAttributeID)
                        .ToListAsync();
                    attributeValues.AddRange(values);
                }

                if (promotions.Count != 0)
                {
                    // Thực hiện SaveChanges để tránh bị conflict với khoá ngoại
                    _phoneShopDBContext.Promotion.RemoveRange(promotions);
                    await _phoneShopDBContext.SaveChangesAsync();
                }
                _phoneShopDBContext.ProductAttributeValue.RemoveRange(attributeValues);
                _phoneShopDBContext.ProductAttribute.RemoveRange(productAttributes);
                _phoneShopDBContext.Product.Remove(product);

                returnData.ReturnCode = (int)ReturnCode.Success;
                returnData.ReturnMsg = "Xoá dữ liệu thành công.";
                return returnData;
            }
            catch (Exception ex)
            {
                returnData.ReturnCode = (int)ReturnCode.Exception;
                returnData.ReturnMsg = ex.Message;
                return returnData;
            }
        }

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
                returnData.Product = product;

                var productAttributes = await _phoneShopDBContext.ProductAttribute
                    .Where(x => x.ProductID == id)
                    .ToListAsync();

                if (productAttributes == null || productAttributes.Count <= 0)
                {
                    returnData.ReturnCode = (int)ReturnCode.EqualNull;
                    returnData.ReturnMsg = $"Sản phẩm với id {id} không có thuộc tính nào.";
                    return returnData;
                }
                returnData.Attributes = productAttributes;

                foreach (var item in productAttributes)
                {
                    returnData.AttributeValues = await _phoneShopDBContext.ProductAttributeValue
                        .Where(x => x.ProductAttributeID == id)
                        .ToListAsync();
                }
                
                returnData.ReturnCode = (int)ReturnCode.Success;
                returnData.ReturnMsg = $"Tìm thấy sản phẩm với id {id}.";
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

                if (attributes.Length <= 0 || attributes == null)
                {
                    returnData.ReturnCode = (int)ReturnCode.Invalid;
                    returnData.ReturnMsg = "Dữ liệu thuộc tính của sản phẩm không hợp lệ.";
                    return returnData;
                }

                for (int i = 0; i < attributes.Length; i++)
                {
                    var attributeDetails = attributes[i].Split("_");
                    if (attributeDetails == null || attributeDetails.Length <= 1)
                    {
                        returnData.ReturnCode = (int)ReturnCode.Invalid;
                        returnData.ReturnMsg = "Dữ liệu thuộc tính của sản phẩm không hợp lệ.";
                        return returnData;
                    }

                    var attributeName = attributeDetails[0];
                    var attributeValues = attributeDetails.Skip(1).ToList();
                    if (attributeName.IsValidated()
                        || attributeValues.Any(x => x.IsValidated())
                        )
                    {
                        returnData.ReturnCode = (int)ReturnCode.Invalid;
                        returnData.ReturnMsg = "Dữ liệu thuộc tính của sản phẩm không hợp lệ.";
                        return returnData;
                    }

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

        public Task<ReturnData> UpdateProduct(ProductRequestAddUpdateData requestData)
        {
            throw new NotImplementedException();
        }

        private async Task<ReturnData> AddAttributeValue(ProductAttribute attribute, List<string> attributeValues)
        {
            var returnData = new ProductAddReturnData();
            try
            {
                for (int i = 0; i < attributeValues.Count; i++)
                {
                    var values = attributeValues[i].Split(",");

                    if (values.Length != 0
                        || !values[0].IsValidated()
                        || !values[1].IsNumber()
                        || !values[2].IsNumber()
                        || !values[3].IsNumber()
                        )
                    {
                        returnData.ReturnCode = (int)ReturnCode.Invalid;
                        returnData.ReturnMsg = "Dữ liệu đầu vào của giá trị thuộc tính không hợp lệ.";
                        return returnData;
                    }

                    var valueName = values[0];
                    var valueQuantity = Convert.ToInt32(values[1]);
                    var valuePrice = Convert.ToInt32(values[2]);
                    var valuePriceSale = Convert.ToInt32(values[3]);

                    if (valueQuantity < 0
                        || valuePrice < 0
                        || valuePriceSale < 0
                        )
                    {
                        returnData.ReturnCode = (int)ReturnCode.MinimumRequired;
                        returnData.ReturnMsg = "Số lượng hoặc giá không hợp lệ.";
                        return returnData;
                    }
                    if (valuePriceSale > valuePrice)
                    {
                        returnData.ReturnCode = (int)ReturnCode.Invalid;
                        returnData.ReturnMsg = "Giá sale không thể lớn hơn giá gốc.";
                        return returnData;
                    }

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

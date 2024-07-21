using Microsoft.AspNetCore.Mvc;
using PhoneShop.Commonlibs;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.UnitOfWork;

namespace PhoneShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IUnitOfWork unitOfWork, IConfiguration configuration) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IConfiguration _configuration = configuration;

        [HttpPost("GetAll")]
        public async Task<ActionResult> GetAll()
        {
            var listProducts = new List<Product>();
            try
            {
                listProducts = await _unitOfWork._productServices.GetProducts();
                return Ok(listProducts);
            }
            catch (Exception)
            {
                return Ok(listProducts);
            }
        }

        [HttpPost("Get")]
        public async Task<ActionResult> PagedGet(ProductRequestGetData requestData)
        {
            var returnData = await _unitOfWork._productServices.GetProducts(requestData);
            return Ok(returnData);
        }

        [HttpPost("Add")]
        public async Task<ActionResult> ProductAdd(ProductRequestData requestData)
        {
            var returnData = new ReturnData();

            try
            {
                if (requestData == null
                    || Validation.IsName(requestData.Product.ProductName)
                    || !Validation.IsContainHTMLTags(requestData.Product.ProductDescription)
                    )
                {
                    returnData.ReturnCode = (int)ReturnCode.Invalid;
                    returnData.ReturnMsg = "Dữ liệu về sản phẩm không hợp lệ.";
                    return Ok(returnData);
                }

                if (requestData.Attributes.Exists(
                    x => !Validation.IsName(x.AttributesName))
                    )
                {
                    returnData.ReturnCode = (int)ReturnCode.Invalid;
                    returnData.ReturnMsg = "Dữ liệu về thuộc tính không hợp lệ";
                    return Ok(returnData);
                }

                if (requestData.AttributeValues.Exists(x =>
                        !Validation.IsName(x.AttributeValuesName)
                        || x.Quantity < 0
                        || x.Price < 0
                        || x.PriceSale < 0)
                    )
                {
                    returnData.ReturnCode = (int)ReturnCode.Invalid;
                    returnData.ReturnMsg = "Dữ liệu về giá trị của thuộc tính không hợp lệ";
                    return Ok(returnData);
                }

                var brandList = await _unitOfWork._BrandServices.BrandsGetList();
                if (!brandList.Exists(x => x.BrandID == requestData.Product.BrandID))
                {
                    returnData.ReturnCode = (int)ReturnCode.NotExist;
                    returnData.ReturnMsg = "Nhãn hàng này không tồn tại!";
                    return Ok(returnData);
                }

                // Todo: Thêm phần kiểm tra categoryId

                
            }
            catch (Exception ex)
            {
                returnData.ReturnCode = (int)ReturnCode.Exception;
                returnData.ReturnMsg = ex.Message;
                return Ok(returnData);
            }

            return Ok(returnData);
        }
    }
}

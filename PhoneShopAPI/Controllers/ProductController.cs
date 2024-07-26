using Microsoft.AspNetCore.Mvc;
using PhoneShop.Commonlibs;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.UnitOfWork;
using CommonLibs;

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
            var returnData = new ProductGetReturnData();
            var pageSize = 5;

            try
            {
                var products = await _unitOfWork._productServices.GetProducts();
                if (requestData == null || requestData.PageNumber <= 0)
                {
                    requestData = new ProductRequestGetData
                    {
                        PageNumber = 1
                    };
                }

                if (!string.IsNullOrEmpty(requestData.CreatedDate))
                {
                    products = products.FindAll(x =>
                        x.CreatedDate.GetValueOrDefault().ToShortDateString() ==
                        requestData.CreatedDate
                    );
                }

                if (requestData.BrandId != null)
                {
                    products = products.FindAll(x =>
                        x.BrandID == requestData.BrandId
                    );
                }

                if (requestData.CategoryId != null)
                {
                    products = products.FindAll(x =>
                        x.CategoryID == requestData.CategoryId
                    );
                }

                returnData.Products = products.ToPagedList((int)requestData.PageNumber, pageSize);

                returnData.Brands = await _unitOfWork._BrandServices.BrandsGetList();
                returnData.Categories = await _unitOfWork._categoryServices.GetAllCategories();

                returnData.CurrentBrand = requestData.BrandId;
                returnData.CurrentCategory = requestData.CategoryId;
                returnData.CurrentPage = (int)requestData.PageNumber;
                returnData.MaxPageCount = products.Count / pageSize;

                returnData.ReturnCode = (int)ReturnCode.Success;
                returnData.ReturnMsg = "Lấy dữ liệu thành công.";
                return Ok(returnData);
            }
            catch (Exception ex)
            {
                returnData.ReturnCode = (int)ReturnCode.Exception;
                returnData.ReturnMsg = ex.Message;
                return Ok(returnData);
            }
        }

        [HttpPost("Add")]
        public async Task<ActionResult> ProductAdd(ProductRequestData requestData)
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
                    return Ok(returnData);
                }


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

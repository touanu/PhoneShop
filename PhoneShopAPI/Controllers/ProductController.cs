﻿using Microsoft.AspNetCore.Mvc;
using PhoneShop.Commonlibs;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.UnitOfWork;
using CommonLibs;
using Newtonsoft.Json;
using PhoneShopAPI.Models;
using PhoneShopAPI.Filter;

namespace PhoneShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IUnitOfWork unitOfWork, IConfiguration configuration) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IConfiguration _configuration = configuration;

        // Các route chỉ trả về dữ liệu
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

                if (requestData.ProductId != null && requestData.ProductId != -1)
                {
                    products = products.FindAll(x =>
                        x.ProductID == (int)requestData.ProductId
                    );
                }

                if (requestData.ProductName.IsValidated())
                {
                    products = products.FindAll(x =>
                        x.ProductName.Equals(requestData.ProductName, 
                                        StringComparison.CurrentCultureIgnoreCase)
                    );
                }

                if (requestData.BrandId != null && requestData.BrandId != -1)
                {
                    products = products.FindAll(x =>
                        x.BrandID == requestData.BrandId
                    );
                }

                if (requestData.CategoryId != null && requestData.CategoryId != -1)
                {
                    products = products.FindAll(x =>
                        x.CategoryID == requestData.CategoryId
                    );
                }

                returnData.Products = products.ToPagedList(requestData.PageNumber, pageSize);

                returnData.Brands = await _unitOfWork._BrandServices.BrandGetAll();
                returnData.Categories = await _unitOfWork._categoryServices.GetAllCategories();

                returnData.CurrentBrand = requestData.BrandId ?? -1;
                returnData.CurrentCategory = requestData.CategoryId ?? -1;
                returnData.CurrentPage = requestData.PageNumber;
                returnData.MaxPageCount = (products.Count + (pageSize - 1)) / pageSize;

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
        [HttpPost("AddViewData")]
        public async Task<IActionResult> ProductAddViewData()
        {
            var returnData = new ProductAddViewReturnData();

            try
            {
                returnData.Brands = await _unitOfWork._BrandServices.BrandGetAll();
                returnData.Categories = await _unitOfWork._categoryServices.GetAllCategories();
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
        [HttpPost("UpdateViewData")]
        public async Task<IActionResult> ProductUpdateViewData(ProductRequestUpdateView requestData)
        {
            var returnData = new ProductUpdateViewReturnData();

            try
            {
                var productGetReturnData = await _unitOfWork._productServices.GetProductById(requestData.ProductId);

                if (productGetReturnData.ReturnCode < 0)
                {
                    returnData.ReturnCode = productGetReturnData.ReturnCode;
                    returnData.ReturnMsg = productGetReturnData.ReturnMsg;
                    return Ok(returnData);
                }
                returnData.Product = productGetReturnData.Product;
                returnData.Attributes = productGetReturnData.Attributes;
                returnData.AttributeValues = productGetReturnData.AttributeValues;

                returnData.Brands = await _unitOfWork._BrandServices.BrandGetAll();
                returnData.Categories = await _unitOfWork._categoryServices.GetAllCategories();
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

        // Các route tương tác với database
        [HttpPost("Add")]
        public async Task<IActionResult> ProductAdd(ProductRequestAddUpdateData requestData)
        {
            var returnData = new ReturnData();

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
                    return Ok(returnData);
                }

                var response = await UploadProductImages(requestData.Images);

                if (response.ReturnCode < 0)
                {
                    _unitOfWork.Dispose();
                    returnData.ReturnCode = response.ReturnCode;
                    returnData.ReturnMsg = response.ReturnMsg;
                    return Ok(returnData);
                }

                requestData.Images = response.ReturnMsg;
                var insertResponse = await _unitOfWork._productServices.InsertProduct(requestData);

                if (insertResponse.ReturnCode < 0)
                {
                    _unitOfWork.Dispose();
                    returnData.ReturnCode = insertResponse.ReturnCode;
                    returnData.ReturnMsg = insertResponse.ReturnMsg;
                    return Ok(returnData);
                }

                _unitOfWork.SaveChange();
                returnData.ReturnCode = (int)ReturnCode.Success;
                returnData.ReturnMsg = "Thêm dữ liệu thành công.";
                return Ok(returnData);
            }
            catch (Exception ex)
            {
                _unitOfWork.Dispose();
                returnData.ReturnCode = (int)ReturnCode.Exception;
                returnData.ReturnMsg = ex.Message;
                return Ok(returnData);
            }
        }
        [HttpPost("Delete")]
        public async Task<IActionResult> ProductDelete(ProductRequestDeleteData requestData)
        {
            var returnData = new ReturnData();

            try
            {
                if (requestData == null)
                {
                    returnData.ReturnCode = (int)ReturnCode.Invalid;
                    returnData.ReturnMsg = "Dữ liệu đầu vào không hợp lệ.";
                    return Ok(returnData);
                }

                await _unitOfWork._productServices.DeleteProduct(requestData);
                _unitOfWork.SaveChange();

                returnData.ReturnCode = (int)ReturnCode.Success;
                returnData.ReturnMsg = "Xoá dữ liệu thành công.";
                return Ok(returnData);
            }
            catch (Exception ex)
            {
                _unitOfWork.Dispose();
                returnData.ReturnCode = (int)ReturnCode.Exception;
                returnData.ReturnMsg = ex.Message;
                return Ok(returnData);
            }
        }
        [HttpPost("Update")]
        public async Task<IActionResult> ProductUpdate(ProductRequestAddUpdateData requestData)
        {
            var returnData = new ReturnData();

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
                    return Ok(returnData);
                }

                var uploadImageReponse = await UploadProductImages(requestData.Images);
                if (uploadImageReponse.ReturnCode < 0)
                {
                    returnData.ReturnCode = uploadImageReponse.ReturnCode;
                    returnData.ReturnMsg = uploadImageReponse.ReturnMsg;
                    return Ok(returnData);
                }
                requestData.Images = uploadImageReponse.ReturnMsg;

                var response = await _unitOfWork._productServices.UpdateProduct(requestData);
                if (response.ReturnCode < 0)
                {
                    return Ok(response);
                }

                _unitOfWork.SaveChange();
                returnData.ReturnCode = (int) ReturnCode.Success;
                returnData.ReturnMsg = "Cập nhật dữ liệu thành công.";
                return Ok(returnData);
            }
            catch (Exception ex)
            {
                _unitOfWork.Dispose();
                returnData.ReturnCode = (int) ReturnCode.Exception;
                returnData.ReturnMsg = ex.Message;
                return Ok(returnData);
            }
        }

        // Method chung
        private async Task<ReturnData> UploadProductImages(string Base64Images)
        {
            var returnData = new ReturnData();

            var baseurl = _configuration["URL:MEDIA_URL"] ?? "";
            var url = "api/Media/Upload";
            var secretKey = _configuration["Sercurity:SecretKey"] ?? "";
            var listOfImages = Base64Images.Split('_');
            var imageNames = "";

            foreach (var item in listOfImages)
            {
                if (item.EndsWith(".png"))
                {
                    imageNames += item + ",";
                    continue;
                }

                var plaintext = item + secretKey;
                var sign = Security.MD5(plaintext);

                var requestUpload = new UploadRequestData
                {
                    Base64Image = item,
                    Sign = sign
                };

                var jsonData = JsonConvert.SerializeObject(requestUpload);
                var result = await HttpHelper.HttpSendPost(baseurl, url, jsonData);
                if (string.IsNullOrEmpty(result))
                {
                    returnData.ReturnCode = (int)ReturnCode.Failure;
                    returnData.ReturnMsg = "Không thể upload ảnh lên máy chủ.";
                    return returnData;
                }
                var response = JsonConvert.DeserializeObject<ReturnData>(result);
                if (response == null || response.ReturnCode < 0)
                {
                    returnData.ReturnCode = (int)ReturnCode.Failure;
                    returnData.ReturnMsg = "Không thể upload ảnh lên máy chủ.";
                    return returnData;
                }
                
                imageNames += response.ReturnMsg + ",";
            }

            imageNames = imageNames[..^1];
            returnData.ReturnCode = (int) ReturnCode.Success;
            returnData.ReturnMsg = imageNames;
            return returnData;
        }
    }
}

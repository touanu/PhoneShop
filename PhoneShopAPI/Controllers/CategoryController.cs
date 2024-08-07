﻿using CommonLibs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.UnitOfWork;
using PhoneShopAPI.Filter;
using PhoneShopAPI.Models;

namespace PhoneShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private IConfiguration _configuration;
        private IUnitOfWork _unitOfWork;
        public CategoryController(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }
        [HttpPost("AddCategory")]
        [PhoneShopAuthorize("Add_Category", "INSERT")]
        public async Task<ActionResult> AddCategory(CategoryRequestData requestData)
        {
            CategoryReturnData returnData = new CategoryReturnData();
            //validate
            if (requestData == null
                || requestData.CategoryName == null
                || requestData.DisplayStatus <= 0
                || requestData.IconImages == null)
            {
                returnData.ReturnCode = -1;
                returnData.ReturnMsg = "Dữ liệu đầu vào không hợp lệ!";
                return Ok(returnData);
            }
            // bước 1: gọi media để upload ảnh 

            // Bước 1.1 : khai báo API URL

            var baseurl = _configuration["URL:MEDIA_URL"] ?? "";
            var url = "api/Media/Upload";

            // bƯỚC 1.2: tạo json data ( object sang JSON)

            // kiểm tra xem chữ ký có hợp lệ không ?
            var SecretKey = _configuration["Sercurity:SecretKey"] ?? "";

            var plantext = requestData.IconImages + SecretKey;

            var Sign =Security.MD5(plantext);

            var requestUpload = new UploadRequestData
            {
                Base64Image = requestData.IconImages,
                Sign = Sign
            };

            var jsonData = JsonConvert.SerializeObject(requestUpload);

            // Bước 1.3 : gọi httpclient bên common để post lên api
            var result = await HttpHelper.HttpSendPost(baseurl, url, jsonData);

            // Bước 1.4: nhận dữ liệu về 
            var imageName = "";
            if (!string.IsNullOrEmpty(result))
            {
                var rs = JsonConvert.DeserializeObject<ReturnData>(result);
                if (rs != null && rs.ReturnCode > 0)
                {
                    imageName = rs.ReturnMsg;
                }
            } 


            // bước 2: có ảnh từ bước 1 rồi thì thực hiện lưu xuống db
            requestData.IconImages = imageName;
            var response = await _unitOfWork._categoryServices.AddCategory(requestData);
            _unitOfWork.SaveChange();
            returnData.ReturnCode = response.ReturnCode;
            returnData.ReturnMsg = response.ReturnMsg;
            return Ok(returnData);
        }
        [HttpPost("GetCategory")]
        [PhoneShopAuthorize("Get_Category", "VIEW")]
        public async Task<ActionResult> GetCategory (CategoryRequestData requuestData)
        {
            var lstCategory = new GetCategoryReturnData();
            try
            {
                var media_url  = _configuration["URL:MEDIA_URL"] ?? "";
                media_url = media_url + "Upload/";
                lstCategory = await _unitOfWork._categoryServices.GetCategories(requuestData);
                if (lstCategory.list.Count > 0)
                {
                    foreach (var item in lstCategory.list)
                    {
                        item.IconImages = media_url + item.IconImages;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return Ok(lstCategory);
        }
        [HttpPost("RemoveCategory")]
        public async Task<ActionResult> RemoveCategory(CategoryRequestData requestData)
        {
            ReturnData returnData = new ReturnData();
            try
            {
                //validate du lieu
                if (requestData == null
                    || requestData.CategoryID <= 0)
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "Dữ liệu đầu vào không hợp lệ";
                    return Ok(returnData);
                }

                var response = await _unitOfWork._categoryServices.DeleteCategory(requestData);
                _unitOfWork.SaveChange();
                if (response.ReturnCode <= 0)
                {
                    returnData.ReturnCode = response.ReturnCode;
                    returnData.ReturnMsg = response.ReturnMsg;
                    return Ok(returnData);
                }
                returnData.ReturnCode = 1;
                returnData.ReturnMsg = "xóa danh muc thành công!";
                return Ok(returnData);
            }
            catch (Exception ex)
            {
                returnData.ReturnCode = -969;
                returnData.ReturnMsg = ex.Message;
                return Ok(returnData);
            }
        }
    }
}

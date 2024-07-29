using CommonLibs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.IServices;
using PhoneShop.DataAccess.Services;
using PhoneShop.DataAccess.UnitOfWork;
using PhoneShopAPI.Filter;
using PhoneShopAPI.Models;
namespace PhoneShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        public IUnitOfWork _unitOfWork;
        private IConfiguration _configuration;

        public BrandController(IUnitOfWork unitOfWork, IConfiguration configuration )
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        [HttpPost("BrandGetList")]
        [PhoneShopAuthorize("Get_Brand", "VIEW")]
        public async Task<ActionResult> BrandsGetList(BrandRequetsData requetsData)
        {
            var lstBrand = new BrandListReturnData();
            try
            {
                var media_url = _configuration["URL:MEDIA_URL"] ?? "";
                media_url = media_url + "Upload/";
                lstBrand = await _unitOfWork._BrandServices.BrandsGetList(requetsData);
                if (lstBrand.list.Count > 0)
                {
                    foreach (var item in lstBrand.list)
                    {
                        item.IconImages = media_url + item.IconImages;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Ok(lstBrand);
        }

        [HttpPost("BrandInsert")]
        [PhoneShopAuthorize("Add_Brands", "INSERT")]
        public async Task<ActionResult> BrandInsert(BrandInsertRequestData requestData)
        {
            var returnData = new ReturnData();
            try
            {
                // bước 1: gọi media để upload ảnh 

                // Bước 1.1 : khai báo API URL

                var baseurl = _configuration["URL:MEDIA_URL"] ?? "";
                var url = "api/Media/Upload";

                // bƯỚC 1.2: tạo json data ( object sang JSON)


                // kiểm tra xem chữ ký có hợp lệ không ?
                var SecretKey = _configuration["Sercurity:SecretKey"] ?? "";

                var plantext = requestData.IconImages + SecretKey;

                var Sign = Security.MD5(plantext);

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
                var insert = _unitOfWork._BrandServices.BrandInsertUpdate(requestData);
                _unitOfWork.SaveChange();
                returnData.ReturnCode= 1;
                returnData.ReturnMsg = "ok";
                return Ok(returnData);
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        [HttpDelete("DeleteBrand")]
        public async Task<IActionResult> DeleteBrand(Brand_DeleteRequestData requestData)
        {
            var BrandDeleteRtdata = new Brand_DeleteRequestData(); 

            var result = await _unitOfWork._BrandServices.Brand_Delete(BrandDeleteRtdata);
            if (result == null)
            {
                return NotFound();
            }
         
            return NoContent();
        }
    }
}

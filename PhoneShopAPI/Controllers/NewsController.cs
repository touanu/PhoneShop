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
    public class NewsController : ControllerBase
    {
        public IUnitOfWork _unitOfWork;
        private IConfiguration _configuration;

        public NewsController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        [HttpPost("NewGetList")]
        [PhoneShopAuthorize("Get_New", "VIEW")]
        public async Task<ActionResult> NewGetList(NewRequestDaTa requestDaTa)
        {
            var lstNew = new NewsListReturnData();
            try
            {
                var media_url = _configuration["URL:MEDIA_URL"] ?? "";
                media_url = media_url + "Upload/";
              lstNew = await _unitOfWork._NewServices.NewGetList(requestDaTa);
                if (lstNew.list.Count > 0)
                {
                    foreach (var item in lstNew.list)
                    {
                        item.NewsMain = media_url + item.NewsMain;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Ok(lstNew);
        }

        [HttpPost("NewsInsert")]
        [PhoneShopAuthorize("Add_News", "INSERT")]
        public async Task<ActionResult> NewsInsert(NewInsertUpdateRequestDaTa requestDaTa)
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

                var plantext = requestDaTa.NewsMain + SecretKey;

                var Sign = Security.MD5(plantext);

                var requestUpload = new UploadRequestData
                {
                    Base64Image = requestDaTa.NewsMain,
                    Sign = Sign
                };

                var jsonData = JsonConvert.SerializeObject(requestUpload);

                // Bước 1.3 : gọi httpclient bên common để post lên api
                var result = await HttpHelper.HttpSendPost(baseurl, url, jsonData);

                // Bước 1.4: nhận dữ liệu về 
                var NewsMain = "";
                if (!string.IsNullOrEmpty(result))
                {
                    var rs = JsonConvert.DeserializeObject<ReturnData>(result);
                    if (rs != null && rs.ReturnCode > 0)
                    {
                        NewsMain = rs.ReturnMsg;
                    }
                }

                // bước 2: có ảnh từ bước 1 rồi thì thực hiện lưu xuống db
                requestDaTa.NewsMain = NewsMain;
                var insert = _unitOfWork._NewServices.NewInsertUpdate(requestDaTa);
                _unitOfWork.SaveChange();
                returnData.ReturnCode = 1;
                returnData.ReturnMsg = "ok";
                return Ok(returnData);
            }
            catch (Exception ex)
            {

                throw;
            }


        }

    }
}


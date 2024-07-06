
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.IServices;
using PhoneShop.DataAccess.Services;
using PhoneShop.DataAccess.UnitOfWork;
using PhoneShop.Models;
using System.Net.Http;

namespace PhoneShop.Controllers
{
    public class BrandController : Controller
    {
        public IUnitOfWork _unitOfWork;
        public IBrandServices _BrandServices;
        private IConfiguration _configuration;

        public BrandController(IUnitOfWork unitOfWork, IConfiguration configuration, IBrandServices _BrandServices, IBrandServices brandServices)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _BrandServices = brandServices;
        }
        public IActionResult Index()
        {
            return View();
        }


       

        public async Task<JsonResult> BrandList(BrandRequetsData requetsData) 
        {

            var model = new PhoneShopResponse();
            try
            {
                if (requetsData == null
                    || string.IsNullOrEmpty(requetsData.BrandName))
                {
                    model.ResponseCode = -1;
                    model.ResponseMessage = "Dữ liệu không được trống";
                    return Json(model);
                }


                // Bước 1.1 : khai báo API URL

                var baseurl = _configuration["API_URL:URL"] ?? "";
                var url = "api/Product/ProductInsert";

                // bƯỚC 1.2: tạo json data ( object sang JSON)
                var jsonData = JsonConvert.SerializeObject(requetsData);

                // Bước 1.3 : gọi httpclient bên common để post lên api
                var result = await PhoneShop.Commonlibs.HttpHelper.HttpSenPost(baseurl, url, jsonData);

                // Bước 1.4: nhận dữ liệu về 
                var imageName = "";
                if (!string.IsNullOrEmpty(result))
                {
                    var rs = JsonConvert.DeserializeObject<ReturnData>(result);

                }

                model.ResponseCode = 1;
                model.ResponseMessage = "";
                return Json(model);
            }
            catch (Exception ex)
            {

                throw;
            }

            return Json(model);

        }

        public async Task<JsonResult> Brand_Delete(Brand_DeleteRequestData requestData )
        {
            var returnData = new ReturnData();
            try
            {
                if (requestData.BrandName == null)
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "Dữ liệu vào không hợp lệ!";
                    return Json(returnData);
                }
                returnData = await _BrandServices.Brand_Delete(requestData);
                return Json(returnData);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}


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


       

        public async Task<JsonResult> BrandInsert(BrandInsertRequestData requetsData) 
        {

            var returnData = new BrandInsertReturnData();
            try

            {
                // Bước 1 : khai báo API URL

                var baseurl = _configuration["API_URL:URL"] ?? "";
                var url = "api/Brands/Brand";

                // bƯỚC 2: tạo json data ( object sang JSON)
                var jsonData = JsonConvert.SerializeObject(requetsData);

                // Bước 3 : gọi httpclient bên common để post lên api
                var result = await PhoneShop.Commonlibs.HttpHelper.HttpSenPost(baseurl, url, jsonData);

                // Bước 4: nhận dữ liệu về 
                if (!string.IsNullOrEmpty(result))
                {
                    var BrandReq = new BrandInsertRequestData();
                    var response = JsonConvert.DeserializeObject<BrandInsertRequestData>(result);
                    BrandReq.BrandName = response.BrandName;
                   BrandReq.IconImages= response.IconImages;
                    BrandReq.BrandID = response.BrandID;
                    returnData = (BrandInsertReturnData)await _BrandServices.BrandInsertUpdate(BrandReq);
                }

                return Json(returnData);

            }
            catch (Exception ex)
            {
                return Json(returnData);
            }
            return Json(returnData);

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

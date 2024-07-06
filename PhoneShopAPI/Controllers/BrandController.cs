using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.IServices;
using PhoneShop.DataAccess.Services;
using PhoneShop.DataAccess.UnitOfWork;
using PhoneShop.Models;
namespace PhoneShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        public IUnitOfWork _unitOfWork;
        public IBrandServices _BrandServices;
        private IConfiguration _configuration;

        public BrandController(IUnitOfWork unitOfWork, IConfiguration configuration , IBrandServices _BrandServices, IBrandServices brandServices)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _BrandServices = brandServices;
        }

        [HttpPost("BrandGetList")]
        public async Task<ActionResult> BrandsGetList()
        {
            var lstBrand = new List<Brands>();
            try
            {
               lstBrand = await _unitOfWork._BrandServices.BrandsGetList();
            }
            catch (Exception ex)
            {

                throw;
            }

            return Ok(lstBrand);
        }

        [HttpPost("BrandInsert")]
        public async Task<ActionResult> BrandInsert(BrandInsertRequestData requestData)
        {
            var returnData = new ReturnData();
            try
            {
                // bước 1: gọi media để upload ảnh 

                // Bước 1.1 : khai báo API URL

                var baseurl = _configuration["URL:API_URL"] ?? "";
                var url = "api/Media/Upload";

                // bƯỚC 1.2: tạo json data ( object sang JSON)

                // kiểm tra xem chữ ký có hợp lệ không ?
               

                var jsonData = JsonConvert.SerializeObject(requestData);

                // Bước 1.3 : gọi httpclient bên common để post lên api
                var result = await PhoneShop.Commonlibs.HttpHelper.HttpSenPost(baseurl, url, jsonData);

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

                var BrandinsertRq = new BrandInsertRequestData
                {
                   IconImages  = imageName,
                    BrandName= "",
                };
                var insert = _unitOfWork._BrandServices.BrandInsertUpdate(BrandinsertRq);
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

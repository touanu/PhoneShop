using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.UnitOfWork;

namespace PhoneShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private IConfiguration _configuration;
        public PromotionController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        [HttpPost("AddPromotion")]
        public async Task<ActionResult> AddPromotion (PromotionsRequestData requestData)
        {
            GetPromotionsReturnData returnData= new GetPromotionsReturnData();
            try
            {
                if (requestData == null
                    ||string.IsNullOrEmpty(requestData.PromotionName)
                    ||requestData.CustomerID<0
                    ||requestData.ProductID<0
                    ||requestData.Quantity<0
                    )
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "Dữ liệu đầu vào không hợp lệ";
                    return Ok(returnData);
                }
                var response = await _unitOfWork._promotionsServices.AddPromotions(requestData);
                _unitOfWork.SaveChange();
                returnData.ReturnMsg = response.ReturnMsg;
                returnData.ReturnCode=response.ReturnCode;
                returnData.promotion=response.promotion;
                return Ok(returnData);
            }
            catch (Exception ex)
            {
                returnData.ReturnCode = -969;
                returnData.ReturnMsg = ex.Message;
                return Ok(returnData);
            }
        }
        [HttpPost("GetPromotion")]
        public async Task<ActionResult> GetPromotion(PromotionsRequestData requestData )
        {
            var list = _unitOfWork._promotionsServices.GetPromotions(requestData);
            return Ok(list);
        }
    }
}

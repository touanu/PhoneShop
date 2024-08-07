﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.UnitOfWork;
using PhoneShopAPI.Filter;

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
        [PhoneShopAuthorize("Add_Promotions","INSERT")]
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
        [PhoneShopAuthorize("Get_Promotions", "VIEW")]
        public async Task<ActionResult> GetPromotion(PromotionsRequestData requestData )
        {
            var list = await _unitOfWork._promotionsServices.GetPromotions(requestData);
            return Ok(list);
        }
        [HttpPost("RemovePromotion")]
        public async Task<IActionResult> RemovePromotion(PromotionsRequestData requestData)
        {
            ReturnData returnData = new ReturnData();
            try
            {
                //validate du lieu
                if (requestData == null
                    || requestData.PromotionID <= 0)
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "Dữ liệu đầu vào không hợp lệ";
                    return Ok(returnData);
                }

                var response = await _unitOfWork._promotionsServices.DeletePromotions(requestData);
                _unitOfWork.SaveChange();
                if (response.ReturnCode <= 0)
                {
                    returnData.ReturnCode = response.ReturnCode;
                    returnData.ReturnMsg = response.ReturnMsg;
                    return Ok(returnData);
                }
                returnData.ReturnCode = 1;
                returnData.ReturnMsg = "xóa Khuyến mãi thành công!";
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

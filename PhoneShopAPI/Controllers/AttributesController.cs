using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.UnitOfWork;
using System.IdentityModel.Tokens.Jwt;

namespace PhoneShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttributesController : ControllerBase
    {
        private IConfiguration _configuration;
        private IUnitOfWork _unitOfWork;
        public AttributesController(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("AddAttribute")]
        public async Task<ActionResult> AddAttribute(AttributesRequestData requestData)
        {
            ReturnDataReturnAttributes returnData =new ReturnDataReturnAttributes();
            try
            {
                //validate
                if(requestData.AttributesNameValue==null
                    ||requestData.AttributeValuestring==null)
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "Dữ liệu đầu vào không hợp lệ!";
                    return Ok(returnData);
                }
                var response = await _unitOfWork._Attributesservices.AddAttributes(requestData);
                _unitOfWork.SaveChange();
                returnData.ReturnCode = response.ReturnCode;
                returnData.ReturnMsg = response.ReturnMsg;
                return Ok(returnData);
            }
            catch (Exception ex)
            {

                returnData.ReturnCode = -969;
                returnData.ReturnMsg = ex.Message;
                return Ok(returnData);
            }
        }
        [HttpPost("GetAttribute")]
        public async Task<ActionResult> GetAttribute(AttributesResponseData requuestData)
        {
            var list = _unitOfWork._Attributesservices.GetAttributes(requuestData);

            return Ok(list);
        }
    }
}

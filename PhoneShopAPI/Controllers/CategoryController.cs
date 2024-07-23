using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.UnitOfWork;

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
            var response = await _unitOfWork._categoryServices.AddCategory(requestData);
            _unitOfWork.SaveChange();
            returnData.ReturnCode = response.ReturnCode;
            returnData.ReturnMsg = response.ReturnMsg;
            return Ok(returnData);
        }
        [HttpPost("GetCategory")]
        public async Task<ActionResult> GetCategory(CategoryRequestData requuestData)
        {
            var list = await _unitOfWork._categoryServices.AddCategory(requuestData);

            return Ok(list);
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.UnitOfWork;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PhoneShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private IConfiguration _configuration;
        public AccountController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }


        [HttpPost("Login")]
        public async Task<ActionResult> Login(AccountRequestData requestData)
        {
            var returnData = new ReturnDataReturnAccount();
            try
            {
                // Bước 1: gọi đăng nhập để lấy thông tin tài khoản
                // Bước 1.1 : Validate dữ lioeeuj ddaafui vào

                if (requestData == null
                    || string.IsNullOrEmpty(requestData.UserName)
                    || string.IsNullOrEmpty(requestData.PassWord))
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "Dữ liệu đầu vào không hợp lệ";
                    return Ok(returnData);
                }

                // Bước 1.2 : text 123456 -> n2zmy77Xfb4cnD7ca8VYip/kGpT4q+1gcONNT5 dùng
                //  Thuật toán mã hóa : RSA265
                var salt = _configuration["Sercurity:Salt"] ?? "";
                var passwordHash = PhoneShop.Commonlibs.Sercuritys.EncryptPassword(requestData.PassWord, salt);


                // Bước 1.2 : gọi UnitOfWork lấy thông tin tài khoản

                requestData.PassWord = passwordHash;
                var response = await _unitOfWork._accountServices.AccountLogin(requestData);

                if (response.ReturnCode <= 0)
                {
                    returnData.ReturnCode = response.ReturnCode;
                    returnData.ReturnMsg = response.ReturnMsg;
                    return Ok(returnData);
                }

                //bƯỚC 2: Dùng Claims để tạo token từ tài khoản có được ở bước 1.2
                var account = response.Accounts;

                var authClaims = new List<Claim> {
                    new Claim(ClaimTypes.Name, account.UserName),
                    new Claim(ClaimTypes.Sid,account.AccountID.ToString())
                    };

                var newToken = CreateToken(authClaims);

                //Bước 3: Trả về Token cho Client

                returnData.ReturnCode = 1;
                returnData.ReturnMsg = "Đăng nhập thành công!";
                returnData.Accounts = response.Accounts;
                returnData.Token = new JwtSecurityTokenHandler().WriteToken(newToken);

                return Ok(returnData);
            }
            catch (Exception ex)
            {
                returnData.ReturnCode = -969;
                returnData.ReturnMsg = ex.Message;
                return Ok(returnData);
            }
        }
        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
        [HttpPost("Register")]
        public async Task<ActionResult> Register(AccountRequestData requestData)
        {
            ReturnDataReturnAccount returnData = new ReturnDataReturnAccount();
            try
            {
                //validate du lieu
                if (requestData == null
                    || string.IsNullOrEmpty(requestData.UserName)
                    || string.IsNullOrEmpty(requestData.PassWord)
                    || string.IsNullOrEmpty(requestData.FristName)
                    || string.IsNullOrEmpty(requestData.LastName))
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "Dữ liệu đầu vào không hợp lệ";
                    return Ok(returnData);
                }
                //ma hoa password
                var salt = _configuration["Sercurity:Salt"] ?? "";
                var passwordHash = PhoneShop.Commonlibs.Sercuritys.EncryptPassword(requestData.PassWord, salt);

                requestData.PassWord = passwordHash;
                var response = await _unitOfWork._accountServices.AddCustomer(requestData);

                if (response.ReturnCode <= 0)
                {
                    returnData.ReturnCode = response.ReturnCode;
                    returnData.ReturnMsg = response.ReturnMsg;
                    return Ok(returnData);
                }
                returnData.ReturnCode = 1;
                returnData.ReturnMsg = "Đăng ký thành công!";
                returnData.customers = response.customers;
                return Ok(returnData);
            }
            catch (Exception ex)
            {
                returnData.ReturnCode = -969;
                returnData.ReturnMsg = ex.Message;
                return Ok(returnData);
            }
        }
        [HttpPost("RemoveCustomer")]
        public async Task<ActionResult> RemoveCustomer(AccountRequestData requestData)
        {
            ReturnDataReturnAccount returnData = new ReturnDataReturnAccount();
            try
            {
                //validate du lieu
                if (requestData == null
                    || requestData.ID <= 0)
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "Dữ liệu đầu vào không hợp lệ";
                    return Ok(returnData);
                }

                var response = await _unitOfWork._accountServices.RemoveCustomerByID(requestData);

                if (response.ReturnCode <= 0)
                {
                    returnData.ReturnCode = response.ReturnCode;
                    returnData.ReturnMsg = response.ReturnMsg;
                    return Ok(returnData);
                }
                returnData.ReturnCode = 1;
                returnData.ReturnMsg = "xóa khách hàng thành công!";
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

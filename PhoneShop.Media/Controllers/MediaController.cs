using Microsoft.AspNetCore.Mvc;
using PhoneShop.Media.Models;
using PhoneShop.DataAccess.DTO;
using System.Drawing;
using CommonLibs;

namespace PhoneShop.Media.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController(IConfiguration configuration) : ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;

        [HttpPost("Upload")]
        public async Task<ActionResult> Upload(UploadRequestData requestData)
        {
            var returnData = new ReturnData();
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (requestData == null
                    || string.IsNullOrEmpty(requestData.Base64Image)
                    || string.IsNullOrEmpty(requestData.Sign))
                {
                    returnData.ReturnCode = (int)ReturnCode.Invalid;
                    returnData.ReturnMsg = "Dữ liệu đầu vào không hợp lệ";
                    return Ok(returnData);
                }

                // kiểm tra xem chữ ký có hợp lệ không ?
                var secretKey = _configuration["Security:SecretKey"] ?? "";
                var plaintext = requestData.Base64Image + secretKey;
                var signature = Security.MD5(plaintext);

                if (signature != requestData.Sign)
                {
                    returnData.ReturnCode = (int)ReturnCode.SignatureInvalid;
                    returnData.ReturnMsg = "Chữ ký không hợp lệ không hợp lệ";
                    return Ok(returnData);
                }

                // Vẽ lại ảnh và lưu vào thư mục
                var path = "Upload";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string imageName = $"{Guid.NewGuid()}.png";
                var imgPath = Path.Combine(path, imageName);

                if (requestData.Base64Image.Contains("data:image"))
                {
                    // Cắt bỏ phần 'Signature' để lấy phần ảnh
                    // Tương tự như Substring()
                    requestData.Base64Image = requestData.Base64Image[
                        (requestData.Base64Image.LastIndexOf(',') + 1)..];
                }

                byte[] imageBytes = Convert.FromBase64String(requestData.Base64Image);
                MemoryStream ms = new(imageBytes, 0, imageBytes.Length);
                ms.Write(imageBytes, 0, imageBytes.Length);

                using (Image image = Image.FromStream(ms, true))
                {
                    image.Save(imgPath, System.Drawing.Imaging.ImageFormat.Png);
                }

                returnData.ReturnCode = (int)ReturnCode.Success;
                returnData.ReturnMsg = imageName;
                return Ok(returnData);
            }
            catch (Exception ex)
            {
                returnData.ReturnCode = (int)ReturnCode.Exception;
                returnData.ReturnMsg = ex.Message;
                return Ok(returnData);
            }
        }
    }
}
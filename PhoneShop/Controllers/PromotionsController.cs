﻿using CommonLibs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.IServices;
using PhoneShop.DataAccess.Services;

namespace PhoneShop.Controllers
{
    public class PromotionsController : Controller
    {
        readonly IConfiguration _configuration;
        readonly IPromotionServices _promotionsServices;
        public PromotionsController(IConfiguration config, IPromotionServices promotionsServices)
        {
            _configuration = config;
            _promotionsServices = promotionsServices;
        }

        public IActionResult AddPromotion()
        {
            return View();
        }
        public async Task<JsonResult> AddPromotions(PromotionsRequestData requestData)
        {
            var returnData = new GetPromotionsReturnData();
            try
            {
                var baseurl = _configuration["API_URL:URL"] ?? "";
                var url = "api/Promotions/AddPromotion";

                // bƯỚC 2: tạo json data ( object sang JSON)
                var jsonData = JsonConvert.SerializeObject(requestData);

                // Bước 3 : gọi httpclient bên common để post lên api
                var result = await HttpHelper.HttpSendPost(baseurl, url, jsonData);

                // Bước 4: nhận dữ liệu về 
                if (!string.IsNullOrEmpty(result))
                {
                    var AttReq = new PromotionsRequestData();
                    var response = JsonConvert.DeserializeObject<PromotionsRequestData>(result);
                    AttReq = response;
                    returnData = await _promotionsServices.AddPromotions(AttReq);
                }
                return Json(returnData);
            }
            catch (Exception ex)
            {
                return Json(returnData);
            }


        }
        public IActionResult GetPromotion()
        {
            return View();
        }
        public async Task<JsonResult> GetPromotions(PromotionsRequestData requestData)
        {
            var list = new List<Promotion>();
            try
            {
                var baseurl = _configuration["API_URL:URL"] ?? "";
                var url = "api/Promotions/GetPromotion";

                // bƯỚC 2: tạo json data ( object sang JSON)
                var jsonData = JsonConvert.SerializeObject(requestData);

                // Bước 3 : gọi httpclient bên common để post lên api
                var result = await HttpHelper.HttpSendPost(baseurl, url, jsonData);

                // Bước 4: nhận dữ liệu về 
                if (!string.IsNullOrEmpty(result))
                {
                    var response = JsonConvert.DeserializeObject<PromotionsRequestData>(result);
                    list= await _promotionsServices.GetPromotions(response);
                }
                return Json(list);
            }
            catch (Exception ex)
            {
                return Json(list);
            }
        }
    }
}

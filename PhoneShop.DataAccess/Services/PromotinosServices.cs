using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.Services
{
    public class PromotinosServices : IPromotionsServices
    {
        PhoneShopDBcontext _dbcontext;
        public PromotinosServices(PhoneShopDBcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<GetPromotionsReturnData> AddPromotions(PromotionsRequestData requestData)
        {
            var returnData = new GetPromotionsReturnData();
            try
            {
                if (requestData == null
                    || requestData.PromotionName == null
                    || requestData.ProductID < 0
                    || requestData.CustomerID < 0
                    || requestData.StartDate == null
                    || requestData.EndDate == null
                    || requestData.TotalDiscount < 0
                    || requestData.MinimumAmount < 0
                    || requestData.PercentageDiscount < 0
                    || requestData.Quantity < 0
                    || requestData.QuantityOneDay < 0)
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "Dữ liệu vào không hợp lệ!";
                    return returnData;
                }
                foreach (var Promo in _dbcontext.Promotion)
                {
                    if (Promo.PromotionName == requestData.PromotionName)
                    {
                        returnData.ReturnCode = -1;
                        returnData.ReturnMsg = "Tên khuyến mãi bị trùng!";
                        return returnData;
                    }
                }
                var T = DateTime.Compare(requestData.EndDate, requestData.StartDate);
                if (T < 0)
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "Dữ liệu vào không hợp lệ!";
                    return returnData;
                }
                var PromotonsReq = new Promotion
                {
                    CustomerID = requestData.CustomerID,
                    StartDate = requestData.StartDate,
                    EndDate = requestData.EndDate,
                    MinimumAmount = requestData.MinimumAmount,
                    PercentageDiscount = requestData.PercentageDiscount,
                    Quantity = requestData.Quantity,
                    ProductID = requestData.ProductID,
                    PromotionName = requestData.PromotionName,
                    QuantityOneDay = requestData.QuantityOneDay,
                    TotalDiscount = requestData.TotalDiscount,
                };
                _dbcontext.Promotion.Add(PromotonsReq);
                returnData.ReturnCode = 1;
                returnData.ReturnMsg = "Thêm khuyến mãi thành công!";
                returnData.promotion = PromotonsReq;
                return returnData;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<ReturnData> DeletePromotions(PromotionsRequestData requestData)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Promotion>> GetPromotions(PromotionsRequestData requestData)
        {
            var list = new List<Promotion>();
            try
            {
                list = _dbcontext.Promotion.ToList();
                if (!string.IsNullOrEmpty(requestData.PromotionName))
                {
                    list = list.FindAll(s => s.PromotionName.ToLower().Contains(requestData.PromotionName.ToLower())).ToList();
                }
                if (requestData.ProductID > 0)
                {
                    var Products = _dbcontext.Products.Find(requestData.ProductID);
                    if (Products != null)
                    {
                        list = list.FindAll(s => s.ProductID == Products.ProductID);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<ReturnData> UpdateTimePromotions(PromotionsRequestData requestData)
        {
            var returnData = new ReturnData();
            try
            {
                if (requestData == null
                    || requestData.PromotionName == null
                    || requestData.ProductID < 0
                    || requestData.CustomerID < 0
                    || requestData.StartDate == null
                    || requestData.EndDate == null
                    || requestData.TotalDiscount < 0
                    || requestData.MinimumAmount < 0
                    || requestData.PercentageDiscount < 0
                    || requestData.Quantity < 0
                    || requestData.QuantityOneDay < 0)
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "Dữ liệu vào không hợp lệ!";
                    return returnData;
                }
                var p = new Promotion();
                foreach (var Promo in _dbcontext.Promotion)
                {
                    if (Promo.PromotionName == requestData.PromotionName)
                    {
                        p = Promo; break;
                    }
                }
                if (p == null)
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "Không có khuyến mãi cần cập nhật!";
                    return returnData;
                }
                var T = DateTime.Compare(requestData.EndDate, requestData.StartDate);
                if (T < 0)
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "Dữ liệu vào không hợp lệ!";
                    return returnData;
                }
                var PromotonsReq = new Promotion
                {
                    CustomerID = requestData.CustomerID,
                    StartDate = requestData.StartDate,
                    EndDate = requestData.EndDate,
                    MinimumAmount = requestData.MinimumAmount,
                    PercentageDiscount = requestData.PercentageDiscount,
                    Quantity = requestData.Quantity,
                    ProductID = requestData.ProductID,
                    PromotionName = requestData.PromotionName,
                    QuantityOneDay = requestData.QuantityOneDay,
                    TotalDiscount = requestData.TotalDiscount,
                };
                _dbcontext.Promotion.Update(PromotonsReq);
                returnData.ReturnCode = 1;
                returnData.ReturnMsg = "Cập nhật khuyến mãi thành công!";
                return returnData;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

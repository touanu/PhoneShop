using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly PhoneShopDBcontext _phoneShopDBContext;
        public async Task<ReturnData> Order_Add(OrderRequestData requestData)
        {
            ReturnData returnData = new();

            try
            {
                if (requestData == null
                    || requestData.Order.CustomerID < 0
                    || requestData.Order.TotalAmount < 0
                    || !requestData.Details.Any(a => a.Price < 0)
                    || !requestData.Details.Any(a => a.Quantity < 0)
                    )
                {
                    returnData.ReturnCode = (int)ReturnCode.Invalid;
                    returnData.ReturnMsg = "Dữ liệu không hợp lệ!";
                    return returnData;
                }

                //_phoneShopDBContext.Orders.Add(requestData.Order);

                foreach (var item in requestData.Details)
                {
                    if (item.PromotionID != null
                        || !_phoneShopDBContext.Promotions.Any(a => a.PromotionID == item.PromotionID))
                    {
                        _phoneShopDBContext.ChangeTracker.Clear();
                        returnData.ReturnCode = (int)ReturnCode.NotExist;
                        returnData.ReturnMsg = "Mã giảm giá không tồn tại!";
                        return returnData;
                    }

                    if (!_phoneShopDBContext.Products.Any(a => a.ProductID == item.ProductID))
                    {
                        _phoneShopDBContext.ChangeTracker.Clear();
                        returnData.ReturnCode = (int)ReturnCode.NotExist;
                        returnData.ReturnMsg = "Sản phẩm này không tồn tại!";
                        return returnData;
                    }

                    if (item.Price < 0
                        || item.Quantity < 0)
                    {
                        _phoneShopDBContext.ChangeTracker.Clear();
                        returnData.ReturnCode = (int)ReturnCode.Invalid;
                        returnData.ReturnMsg = "Dữ liệu không hợp lệ!";
                        return returnData;
                    }


                }

                returnData.ReturnCode = (int)ReturnCode.Success;
                returnData.ReturnMsg = "Thêm dữ liệu thành công!";
                return returnData;
            }
            catch (Exception ex)
            {
                returnData.ReturnCode = (int)ReturnCode.Exception;
                returnData.ReturnMsg = ex.Message;
                return returnData;
                throw;
            }
        }

        public Task<ReturnData> Order_Delete(string orderId)
        {
            throw new NotImplementedException();
        }

        public Task<ReturnData> Order_Update(OrderRequestData requestData)
        {
            throw new NotImplementedException();
        }
    }
}

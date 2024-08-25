using Microsoft.EntityFrameworkCore;
using PhoneShop.Commonlibs;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.Services
{
    public class OrderServices(PhoneShopDBcontext phoneShopDBcontext) : IOrderServices
    {
        private readonly PhoneShopDBcontext _phoneShopDBContext = phoneShopDBcontext;
        public async Task<ReturnData> InsertOrder(OrderRequestData requestData)
        {
            var returnData = new ReturnData();

            try
            {
                if (requestData == null
                    || requestData.CustomerID < 0
                    || requestData.TotalAmount <= 0
                    || !requestData.Details.IsValidated()
                    )
                {
                    returnData.ReturnCode = (int)ReturnCode.Invalid;
                    returnData.ReturnMsg = "Dữ liệu không hợp lệ!";
                    return returnData;
                }

                var customer = await _phoneShopDBContext.Customer.FindAsync(requestData.CustomerID);
                if (customer == null)
                {
                    returnData.ReturnCode = (int)ReturnCode.NotExist;
                    returnData.ReturnMsg = "Khách hàng này không tồn tại!";
                    return returnData;
                }

                var order = new Order
                {
                    CustomerID = requestData.CustomerID,
                    TotalAmount = requestData.TotalAmount,
                    CreatedDate = DateTime.Now,
                    Status = requestData.Status,
                };

                // SaveChanges để nhận được order.OrderID
                await _phoneShopDBContext.Order.AddAsync(order);
                await _phoneShopDBContext.SaveChangesAsync();

                await AddOrderDetail(requestData.Details, customer.CustomerID, order.OrderID);

                returnData.ReturnCode = (int)ReturnCode.Success;
                returnData.ReturnMsg = "Thêm dữ liệu thành công!";
                return returnData;
            }
            catch (Exception ex)
            {
                returnData.ReturnCode = (int)ReturnCode.Exception;
                returnData.ReturnMsg = ex.Message;
                return returnData;
            }
        }

        public async Task<ReturnData> DeleteOrder(int orderId)
        {
            var returnData = new ReturnData();

            try
            {
                if (orderId < 0)
                {
                    returnData.ReturnCode = (int) ReturnCode.Invalid;
                    returnData.ReturnMsg = "Mã đơn hàng không hợp lệ.";
                    return returnData;
                }

                var order = await _phoneShopDBContext.Order.FindAsync(orderId);
                if (order == null)
                {
                    returnData.ReturnCode = (int)ReturnCode.NotExist;
                    returnData.ReturnMsg = "Mã đơn hàng không tồn tại.";
                    return returnData;
                }

                _phoneShopDBContext.Order.Remove(order);
                returnData.ReturnCode = (int)ReturnCode.Success;
                returnData.ReturnMsg = "Xoá dữ liệu thành công.";
                return returnData;
            }
            catch (Exception ex)
            {
                returnData.ReturnCode = (int)ReturnCode.Exception;
                returnData.ReturnMsg = ex.Message;
                return returnData;
            }
        }

        public async Task<ReturnData> UpdateOrder(OrderRequestData requestData)
        {
            var returnData = new ReturnData();
            try
            {
                if (requestData == null
                    || requestData.CustomerID < 0
                    || requestData.TotalAmount <= 0
                    || !requestData.Details.IsValidated()
                    )
                {
                    returnData.ReturnCode = (int)ReturnCode.Invalid;
                    returnData.ReturnMsg = "Dữ liệu không hợp lệ!";
                    return returnData;
                }

                var customer = await _phoneShopDBContext.Customer.FindAsync(requestData.CustomerID);
                if (customer == null)
                {
                    returnData.ReturnCode = (int)ReturnCode.NotExist;
                    returnData.ReturnMsg = "Khách hàng này không tồn tại!";
                    return returnData;
                }

                var order = await _phoneShopDBContext.Order.FindAsync(requestData.OrderID);
                if (order == null)
                {
                    returnData.ReturnCode = (int)ReturnCode.NotExist;
                    returnData.ReturnMsg = "Đơn hàng này không tồn tại!";
                    return returnData;
                }

                order.CustomerID = requestData.CustomerID;
                order.TotalAmount = requestData.TotalAmount;
                order.CreatedDate = DateTime.Now;
                order.Status = requestData.Status;
                
                var orderDetails = await _phoneShopDBContext.OrderDetail.Where(
                        x => x.OrderID == requestData.OrderID
                ).ToListAsync();
                _phoneShopDBContext.OrderDetail.RemoveRange(orderDetails);

                var addDetailsReturnData = await AddOrderDetail(requestData.Details, 
                    customer.CustomerID, order.OrderID);

                returnData.ReturnCode = (int)ReturnCode.Success;
                returnData.ReturnMsg = "Thêm dữ liệu thành công!";
                return returnData;
            }
            catch (Exception ex)
            {
                returnData.ReturnCode = (int)ReturnCode.Exception;
                returnData.ReturnMsg = ex.Message;
                return returnData;
            }
        }

        public async Task<OrderGetByIdReturnData> GetOrderById(int id)
        {
            var returnData = new OrderGetByIdReturnData();

            try
            {
                var order = await _phoneShopDBContext.Order.FindAsync(id);

                if (order == null)
                {
                    returnData.ReturnCode = (int)ReturnCode.EqualNull;
                    returnData.ReturnMsg = $"Hoá đơn với id {id} không tồn tại.";
                    return returnData;
                }
                returnData.Order = order;

                var orderDetails = await _phoneShopDBContext.OrderDetail
                    .Where(x => x.OrderID == id)
                    .ToListAsync();

                if (orderDetails == null || orderDetails.Count <= 0)
                {
                    returnData.ReturnCode = (int)ReturnCode.Success;
                    returnData.ReturnMsg = $"Hoá đơn với id {id} không có mặt hàng nào.";
                    return returnData;
                }
                returnData.Details = orderDetails;

                returnData.ReturnCode = (int)ReturnCode.Success;
                returnData.ReturnMsg = $"Tìm thấy sản phẩm với id {id}.";
                return returnData;
            }
            catch (Exception ex)
            {
                returnData.ReturnCode = (int)ReturnCode.Exception;
                returnData.ReturnMsg = ex.Message;
                return returnData;
            }
        }

        public async Task<List<Order>> GetOrders()
        {
            return await _phoneShopDBContext.Order.ToListAsync();
        }

        // Method dùng chung
        private async Task<ReturnData> AddOrderDetail(string rawDetails, int customerId, int orderId)
        {
            var returnData = new ReturnData();

            try
            {
                var details = rawDetails.Split("_");
                if (details == null || details.Length == 0)
                {
                    returnData.ReturnCode = (int)ReturnCode.Invalid;
                    returnData.ReturnMsg = "Dữ liệu về chi tiết đơn hàng không hợp lệ.";
                    return returnData;
                }

                foreach (var item in details)
                {
                    // values sẽ chứa 4 chỉ số, phân tách bởi dấu ",":
                    // ProductID,PromotionID,Price,Quantity
                    var values = item.Split(",");
                    if (values.Length < 4
                        || values.Any(x => !x.IsNumber()))
                    {
                        returnData.ReturnCode = (int)ReturnCode.Invalid;
                        returnData.ReturnMsg = "Dữ liệu về chi tiết đơn hàng không hợp lệ.";
                        return returnData;
                    }

                    // ProductID
                    var productId = Convert.ToInt32(values[0]);
                    var product = await _phoneShopDBContext.Product.FindAsync(productId);
                    if (product == null)
                    {
                        returnData.ReturnCode = (int)ReturnCode.NotExist;
                        returnData.ReturnMsg = $"Sản phẩm số {productId} không tồn tại!";
                        return returnData;
                    }

                    // PromotionID
                    var promotionId = Convert.ToInt32(values[1]);
                    var promotion = await _phoneShopDBContext.Promotion.FindAsync(promotionId);
                    if (promotion == null)
                    {
                        returnData.ReturnCode = (int)ReturnCode.NotExist;
                        returnData.ReturnMsg = $"Mã giảm giá {promotionId} không tồn tại!";
                        return returnData;
                    }
                    if (DateTime.Now < promotion.StartDate || DateTime.Now > promotion.EndDate
                        || promotion.Quantity <= 0
                        || promotion.ProductID != productId
                        || promotion.CustomerID != customerId
                        )
                    {
                        returnData.ReturnCode = (int)ReturnCode.Invalid;
                        returnData.ReturnMsg = $"Mã giảm giá {promotionId} không hợp lệ!";
                        return returnData;
                    }

                    // Price
                    // Todo: Kiểm tra giá có khớp với giá của sản phẩm + mã giảm giá không
                    var price = Convert.ToInt32(values[2]);
                    if (price < 0)
                    {
                        returnData.ReturnCode = (int)ReturnCode.Invalid;
                        returnData.ReturnMsg = "Giá mặt hàng không hợp lệ!";
                        return returnData;
                    }

                    // Quantity
                    var quantity = Convert.ToInt32(values[3]);
                    if (quantity < 0)
                    {
                        returnData.ReturnCode = (int)ReturnCode.Invalid;
                        returnData.ReturnMsg = "Số lượng sản phẩm không hợp lệ!";
                        return returnData;
                    }

                    var orderDetail = new OrderDetail
                    {
                        OrderID = orderId,
                        ProductID = productId,
                        PromotionID = promotionId,
                        Price = price,
                        Quantity = quantity,
                    };
                    await _phoneShopDBContext.OrderDetail.AddAsync(orderDetail);
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
            }
        }
    }
}

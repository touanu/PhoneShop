using CommonLibs;
using Microsoft.AspNetCore.Mvc;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.UnitOfWork;

namespace PhoneShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IUnitOfWork unitOfWork) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        // Các route chỉ trả về dữ liệu
        [HttpPost("Get")]
        public async Task<ActionResult> PagedGet(OrderGetRequestData requestData)
        {
            var returnData = new OrderGetViewReturnData();
            var pageSize = 5;

            try
            {
                var orders = await _unitOfWork._orderServices.GetOrders();
                if (requestData == null || requestData.PageNumber <= 0)
                {
                    requestData = new OrderGetRequestData
                    {
                        PageNumber = 1
                    };
                }

                if (!string.IsNullOrEmpty(requestData.CreatedDate))
                {
                    orders = orders.FindAll(x =>
                        x.CreatedDate.ToShortDateString() ==
                        requestData.CreatedDate
                    );
                }

                if (requestData.OrderID != null && requestData.OrderID != -1)
                {
                    orders = orders.FindAll(x =>
                        x.OrderID == (int)requestData.OrderID
                    );
                }

                if (requestData.CustomerID != null && requestData.CustomerID != -1)
                {
                    orders = orders.FindAll(x =>
                        x.CustomerID == requestData.CustomerID
                    );
                }

                if (requestData.Status != null && requestData.Status != -1)
                {
                    orders = orders.FindAll(x =>
                        x.Status == requestData.Status
                    );
                }

                var customer = await _unitOfWork._accountServices.GetlistCustomer(
                        new AccountRequestData()
                    );
                if (customer == null || customer.ReturnCode < 0)
                {
                    returnData.ReturnCode = (int)ReturnCode.NotFound;
                    returnData.ReturnMsg = "Không tìm thấy danh sách khách hàng";
                    return Ok(returnData);
                }

                returnData.Orders = orders.ToPagedList(requestData.PageNumber, pageSize);

                returnData.Customers = customer.listcustomer;
                returnData.CurrentCustomer = requestData.CustomerID ?? -1;

                returnData.CurrentStatus = requestData.Status ?? -1;
                returnData.CurrentPage = requestData.PageNumber;
                returnData.MaxPageCount = (orders.Count + (pageSize - 1)) / pageSize;

                returnData.ReturnCode = (int)ReturnCode.Success;
                returnData.ReturnMsg = "Lấy dữ liệu thành công.";
                return Ok(returnData);
            }
            catch (Exception ex)
            {
                returnData.ReturnCode = (int)ReturnCode.Exception;
                returnData.ReturnMsg = ex.Message;
                return Ok(returnData);
            }
        }

        [HttpPost("GetDetail")]
        public async Task<IActionResult> GetDetail(OrderDetailGetRequestData requestData)
        {
            var returnData = new OrderGetReturnData();

            try
            {
                if (requestData == null 
                    || requestData.OrderID == null)
                {
                    returnData.ReturnCode = (int)ReturnCode.Invalid;
                    returnData.ReturnMsg = "Dữ liệu đầu vào không hợp lệ.";
                    return Ok(returnData);
                }

                var getOrderReturn = await _unitOfWork._orderServices.GetOrderById(
                    (int)requestData.OrderID);
                if (getOrderReturn == null
                    || getOrderReturn.ReturnCode < 0
                    || getOrderReturn.Order == null)
                {
                    return Ok(getOrderReturn);
                }
                
                var customer = await _unitOfWork._accountServices.GetCustomerbyID(getOrderReturn.Order.CustomerID);
                if (customer == null)
                {
                    returnData.ReturnCode = (int)ReturnCode.NotFound;
                    returnData.ReturnMsg = "Không tìm thấy khách hàng của đơn hàng này.";
                    return Ok(returnData);
                }

                var address = await _unitOfWork._addressServices.GetAddressByWardID(customer.WardID);
                if (address == null)
                {
                    returnData.ReturnCode = (int)ReturnCode.NotFound;
                    returnData.ReturnMsg = "Không tìm thấy khách hàng của đơn hàng này.";
                    return Ok(returnData);
                }

                returnData.Order = getOrderReturn.Order;
                returnData.Details = getOrderReturn.Details;
                returnData.Customer = customer;
                returnData.CustomerAddress = address;
                returnData.ReturnCode = (int)ReturnCode.Success;
                returnData.ReturnMsg = "Lấy dữ liệu thành công.";
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

namespace PhoneShop.DataAccess.DTO
{
    public class OrderGetReturnData : OrderGetByIdReturnData
    {
        public Customer? Customer { get; set; }
        public AddressReturnData? CustomerAddress { get; set; }
        public List<GetProductReturnData>? Products { get; set; }
    }
    public class OrderGetByIdReturnData : ReturnData
    {
        public Order? Order { get; set; }
        public List<OrderDetail>? Details { get; set; }
    }
    public class OrderGetViewReturnData : ReturnData
    {
        public List<Order>? Orders { get; set; }
        public List<Customer>? Customers { get; set; }
        public int? CurrentCustomer { get; set; }
        public int? CurrentStatus { get; set; }
        public int CurrentPage { get; set; }
        public int MaxPageCount { get; set; }
    }
}

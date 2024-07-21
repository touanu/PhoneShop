namespace PhoneShop.DataAccess.DTO
{
    public class ReturnData
    {
        public int ReturnCode { get; set; }
        public string? ReturnMsg { get; set;}
        public string? Token { get; set; }
    }
    public enum ReturnCode
    {
        Success = 1,
        Failure = -1,
        Exception = -99,
        EqualNull = -2,
        Invalid = -3,
        NotExist = -4,
        AlreadyExist = -5,
        NotAvailable = -6,
        MinimumRequired = -7,
        SignatureInvalid = -8,
    }
    public class ReturnDataReturnAccount : ReturnData
    {
        public Account Account { get; set; }
        public Customer customer{ get; set; }
    }
    public class Result
    {
        public int ReturnCode { get; set; }
        public string? ReturnMsg { get; set; }
    } 
    public class GetAttributeReturndata:ReturnData
    {
        public List< ProductAttribute> list { get; set; }
    }
    public class ReturnDataReturnAttributes:ReturnData
    {
        public List<list> ?list { get; set; }        
    }
    public class ReturnDataReturnpromotion:ReturnData
    {
        public List<Promotion> listpromotion { get; set; }
    }
    public class list
    {
        public int ProductAttributeID { get; set; }
        public int ProductID { get; set; }
        public string ?AttributesName { get; set; }
    }
    public class BrandInsertReturnData : ReturnData
    {
        public Brand brands { get; set; }
    }

    public class Brand_DeleteReturnData : ReturnData
    {
    }
    public class AttributesReturnData : ReturnData 
    {
        public ProductAttributeValue Values { get; set; }
        public ProductAttribute Attributes {  get; set; } 
          
    }
    public class GetProductReturnData : ReturnData
    {
        public Product? Product { get; set; }
    }
    public class GetPromotionsReturnData : ReturnData
    {
        public Promotion? promotion { get; set; }
    }
    public class CategoryReturnData : ReturnData
    {
        public Category? category { get; set; }
    }
    public class ProductGetReturnData : ReturnData
    {
        public required List<Product> Products { get; set; }
        public int CurrentPage { get; set; }
        public int MaxPageCount { get; set; }
    }
}
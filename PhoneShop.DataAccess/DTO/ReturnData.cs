

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
        NotFound = -9,
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
    public class ReturnDataReturnpromotion:ReturnData
    {
        public List<Promotion> ?listpromotion { get; set; }
    }
    public class GetCustomerReturnData : ReturnData
    {
        public List<Customer>? listcustomer { get; set; }
    }
    public class GetCategoryReturnData: ReturnData
    {
        public List<Category> list { get; set; }
    }
    public class BrandInsertReturnData : ReturnData
    {
        public Brand brands { get; set; }
    }
    public class BrandListReturnData : ReturnData
    {
        public List<Brand> list { get; set; }
    }
    public class Brand_DeleteReturnData : ReturnData
    {
    }
    public class GetProductReturnData : ReturnData
    {
        public Product? Product { get; set; }
        public List<ProductAttribute>? Attributes { get; set; }
        public List<ProductAttributeValue>? AttributeValues { get; set; }
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
        public List<Product>? Products { get; set; }
        public List<Brand>? Brands { get; set; }
        public List<Category>? Categories { get; set; }
        public int? CurrentBrand { get; set; }
        public int? CurrentCategory { get; set; }
        public int CurrentPage { get; set; }
        public int MaxPageCount { get; set; }
    }
    public class ProductAddViewReturnData : ReturnData
    {
        public List<Brand>? Brands { get; set; }
        public List<Category>? Categories { get; set; }
    }
    public class ProductAddReturnData : ReturnData
    {

    }
    public class ProductUpdateViewReturnData : ReturnData
    {
        public Product? Product { get; set; }
        public List<ProductAttribute>? Attributes { get; set; }
        public List<ProductAttributeValue>? AttributeValues { get; set; }
        public List<Brand>? Brands { get; set; }
        public List<Category>? Categories { get; set; }
    }
    public class NewsListReturnData : ReturnData
    {
        public List<News>?list { get; set; }

        public static implicit operator NewsListReturnData(List<News> v)
        {
            throw new NotImplementedException();
        }
    }
    public class NewInsertUpdateReturnData : ReturnData
    {

    }
    public class NewDeleteReturnData : ReturnData
    {

    }
    
}
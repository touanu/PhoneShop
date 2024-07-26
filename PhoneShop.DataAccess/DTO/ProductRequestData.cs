﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.DTO
{
    public class ProductRequestData
    {
        public required Product Product { get; set; }
        public required List<ProductAttribute> Attributes { get; set; }
        public required List<ProductAttributeValue> AttributeValues { get; set; }
    }

    public class ProductRequestGetData
    {
        public int PageNumber { get; set; }
        public string? CreatedDate { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
    }
}

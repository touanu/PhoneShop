using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhoneShop.DataAccess.DTO;
using PhoneShop.Models;

namespace PhoneShop.DataAccess
{
    public class PhoneShopDBcontext:DbContext
    {
        public PhoneShopDBcontext(DbContextOptions options):base(options) { }
        public DbSet<Account> Account { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Brand> Brand { get; set; }
        public DbSet<ProductAttribute> ProductAttribute { get; set; }
        public DbSet<ProductAttributeValue> ProductAttributeValue { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<Promotion> Promotion { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Function>? Functions { get; set; }
        public DbSet<UserPermission>? User_Permissions { get; set; }
    }
}
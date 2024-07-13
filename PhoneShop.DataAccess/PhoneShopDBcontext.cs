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
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=DESKTOP-PBSFM7Q;Initial Catalog=PhoneShop;Integrated Security=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;TrustServerCertificate=True");
        //}
        public DbSet<Account> account{ get; set; }
        public DbSet<Customer> customer{ get; set; }

        public DbSet<Brands> brands{ get; set; }

        public DbSet<ProductAttribute> ProductAttribute { get; set; }
        public DbSet<ProductAttributeValue> ProductAttributeValue { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Promotion> Promotion { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Functions>? functions { get; set; }
        public DbSet<User_Permission>? user_Permissions { get; set; }
    }
}
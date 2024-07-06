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
        public DbSet<Accounts> accounts{ get; set; }
        public DbSet<Customers> customers{ get; set; }

        public DbSet<Brands> brands{ get; set; }

        public DbSet<ProductAttributes> productAttributes { get; set; }
        public DbSet<ProductAttributesVallues> attributesVallues { get; set; }
        public DbSet<Oders> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Promotions> Promotions { get; set; }
        public DbSet<Products> Products { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhoneShop.Models;

namespace PhoneShop.DataAccess
{
    public class PhonShopDBcontext:DbContext
    {
        public PhonShopDBcontext(DbContextOptions options):base(options) { }
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=DESKTOP-PBSFM7Q;Initial Catalog=PhoneShop;Integrated Security=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;TrustServerCertificate=True");
        //}
        public DbSet<Accounts> accounts{ get; set; }
        public DbSet<Customers> customers{ get; set; }
    }
}
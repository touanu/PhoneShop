using Microsoft.EntityFrameworkCore;
using PhoneShop.Models;

namespace PhoneShop.DataAccess.DBContext
{
    public class PhoneShopDBcontext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-PBSFM7Q;Initial Catalog=PhoneShop;Integrated Security=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;TrustServerCertificate=True");
        }
        public DbSet<Accounts> accounts { get; set; }
        public DbSet<Customers> customers { get; set; }  
    }
}

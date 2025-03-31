// Data/ShopThoiTrangContext.cs
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopThoiTrang.Models;

namespace ShopThoiTrang.Data
{
    public class ShopThoiTrangContext : IdentityDbContext<ApplicationUser>
    {
        public ShopThoiTrangContext(DbContextOptions<ShopThoiTrangContext> options)
            : base(options)
        {
        }

        public DbSet<QuanAo> QuanAos { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}
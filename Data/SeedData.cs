using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopThoiTrang.Models;
using System;
using System.Linq;

namespace ShopThoiTrang.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ShopThoiTrangContext(
                serviceProvider.GetRequiredService<DbContextOptions<ShopThoiTrangContext>>()))
            {
                // Không thêm dữ liệu mẫu
                // Chỉ kiểm tra và đảm bảo database được tạo
                context.Database.EnsureCreated();
            }
        }
    }
}
// Models/ApplicationUser.cs
using Microsoft.AspNetCore.Identity;

namespace ShopThoiTrang.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } // Tên đầy đủ của người dùng
    }
}
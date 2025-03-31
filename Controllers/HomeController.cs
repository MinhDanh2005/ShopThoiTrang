using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopThoiTrang.Data;
using ShopThoiTrang.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ShopThoiTrang.Controllers
{
    public class HomeController : Controller
    {
        private readonly ShopThoiTrangContext _context;

        public HomeController(ShopThoiTrangContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchString)
        {
            var products = _context.QuanAos.Include(q => q.Images).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                // Lọc sản phẩm theo tiêu đề hoặc thể loại (tùy ý bạn)
                products = products.Where(p => p.Title.Contains(searchString) || p.Genre.Contains(searchString));
            }

            return View(products.ToList());
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var product = _context.QuanAos.FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {
                return Json(new { success = false, message = "Sản phẩm không tồn tại!" });
            }

            var cart = HttpContext.Session.GetString("Cart");
            List<CartItem> cartItems = string.IsNullOrEmpty(cart)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(cart);

            var existingItem = cartItems.FirstOrDefault(item => item.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cartItems.Add(new CartItem
                {
                    ProductId = productId,
                    Title = product.Title,
                    Price = product.Price,
                    Quantity = 1
                });
            }

            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cartItems));
            return Json(new { success = true, message = "Đã thêm vào giỏ hàng!" });
        }

        public IActionResult GetCartCount()
        {
            var cart = HttpContext.Session.GetString("Cart");
            List<CartItem> cartItems = string.IsNullOrEmpty(cart)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(cart);
            var count = cartItems.Sum(c => c.Quantity);
            return Json(count);
        }

        private void UpdateCartCount()
        {
            var cart = HttpContext.Session.GetString("Cart");
            List<CartItem> cartItems = string.IsNullOrEmpty(cart)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(cart);
            ViewBag.CartItemCount = cartItems.Sum(c => c.Quantity);

        }
    }
}
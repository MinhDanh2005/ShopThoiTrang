using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopThoiTrang.Data;
using ShopThoiTrang.Extensions;
using ShopThoiTrang.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShopThoiTrang.Controllers
{
    public class CartController : Controller
    {
        private readonly ShopThoiTrangContext _context;

        public CartController(ShopThoiTrangContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var product = _context.QuanAos.Include(q => q.Images).FirstOrDefault(q => q.Id == productId);
            if (product == null)
            {
                return NotFound();
            }

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var cartItem = cart.FirstOrDefault(c => c.QuanAoId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    QuanAoId = productId,
                    Title = product.Title,
                    Price = product.Price,
                    ImageUrl = product.Images.Any() ? product.Images.First().Url : "/images/default.jpg",
                    Quantity = 1
                });
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var cartItem = cart.FirstOrDefault(c => c.QuanAoId == productId);
            if (cartItem != null)
            {
                cart.Remove(cartItem);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            if (!cart.Any())
            {
                return RedirectToAction("Index");
            }

            HttpContext.Session.Remove("Cart");
            return Content("Thanh toán thành công! Cảm ơn bạn đã mua sắm.");
        }
        public IActionResult GetCartCount()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var count = cart.Sum(c => c.Quantity);
            return Json(count);
        }
    }
}
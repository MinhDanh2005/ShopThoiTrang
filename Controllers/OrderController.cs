using Microsoft.AspNetCore.Mvc;
using ShopThoiTrang.Data;
using ShopThoiTrang.Models;
using System;
using System.Linq;

namespace ShopThoiTrang.Controllers
{
    public class OrderController : Controller
    {
        private readonly ShopThoiTrangContext _context;

        public OrderController(ShopThoiTrangContext context)
        {
            _context = context;
        }

        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (ModelState.IsValid)
            {
                order.OrderDate = DateTime.Now;
                _context.Orders.Add(order);
                _context.SaveChanges();

                return RedirectToAction("Success");
            }
            return View(order);
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}

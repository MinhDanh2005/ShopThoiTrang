using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopThoiTrang.Data;
using ShopThoiTrang.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace ShopThoiTrang.Controllers
{
    public class QuanAosController : Controller
    {
        private readonly ShopThoiTrangContext _context;

        public QuanAosController(ShopThoiTrangContext context)
        {
            _context = context;
        }

        // GET: QuanAos
        public async Task<IActionResult> Index()
        {
            // Lấy danh sách sản phẩm kèm ảnh
            return View(await _context.QuanAos.Include(q => q.Images).ToListAsync());
        }

        // GET: QuanAos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quanAo = await _context.QuanAos
                .Include(q => q.Images) // Lấy cả ảnh
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quanAo == null)
            {
                return NotFound();
            }

            return View(quanAo);
        }

        // GET: QuanAos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: QuanAos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,ReleaseDate,Genre,Price")] QuanAo quanAo, List<IFormFile> imageFiles)
        {
            if (ModelState.IsValid)
            {
                _context.Add(quanAo);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Đã lưu QuanAo với Id: {quanAo.Id}");

                if (imageFiles != null && imageFiles.Count > 0)
                {
                    Console.WriteLine($"Số file nhận được: {imageFiles.Count}");
                    var imageDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    if (!Directory.Exists(imageDir))
                    {
                        Directory.CreateDirectory(imageDir);
                    }

                    foreach (var file in imageFiles)
                    {
                        if (file.Length > 0)
                        {
                            var fileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + Guid.NewGuid() + Path.GetExtension(file.FileName);
                            var filePath = Path.Combine(imageDir, fileName);
                            try
                            {
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await file.CopyToAsync(stream);
                                }
                                var image = new Image
                                {
                                    Url = "/images/" + fileName,
                                    QuanAoId = quanAo.Id
                                };
                                _context.Images.Add(image);
                                Console.WriteLine($"Đã thêm ảnh: {image.Url}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Lỗi khi lưu file: {ex.Message}");
                            }
                        }
                    }
                    try
                    {
                        await _context.SaveChangesAsync();
                        Console.WriteLine("Đã lưu tất cả ảnh vào DB");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi khi lưu DB: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Không nhận được file ảnh nào!");
                }
                return RedirectToAction("Index", "Home");
            }
            return View(quanAo);
        }

        // GET: QuanAos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quanAo = await _context.QuanAos.Include(q => q.Images).FirstOrDefaultAsync(q => q.Id == id);
            if (quanAo == null)
            {
                return NotFound();
            }
            return View(quanAo);
        }

        // POST: QuanAos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price")] QuanAo quanAo, List<IFormFile> imageFiles)
        {
            if (id != quanAo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Cập nhật thông tin sản phẩm
                    _context.Update(quanAo);
                    await _context.SaveChangesAsync();

                    // Xử lý ảnh mới nếu có
                    if (imageFiles != null && imageFiles.Count > 0)
                    {
                        foreach (var file in imageFiles)
                        {
                            if (file.Length > 0)
                            {
                                var fileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + Guid.NewGuid() + Path.GetExtension(file.FileName);
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await file.CopyToAsync(stream);
                                }

                                var image = new Image
                                {
                                    Url = "/images/" + fileName,
                                    QuanAoId = quanAo.Id
                                };
                                _context.Images.Add(image);
                            }
                        }
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuanAoExists(quanAo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home"); // Chuyển hướng về trang chủ
            }
            return View(quanAo);
        }

        // GET: QuanAos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quanAo = await _context.QuanAos
                .Include(q => q.Images) // Lấy cả ảnh để hiển thị
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quanAo == null)
            {
                return NotFound();
            }

            return View(quanAo);
        }

        // POST: QuanAos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quanAo = await _context.QuanAos.Include(q => q.Images).FirstOrDefaultAsync(q => q.Id == id);
            if (quanAo != null)
            {
                // Xóa các ảnh liên quan
                foreach (var image in quanAo.Images)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + image.Url);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath); // Xóa file vật lý
                    }
                }
                _context.QuanAos.Remove(quanAo); // Xóa sản phẩm (cascade sẽ xóa ảnh trong specifically includes trong EF Core tự động xóa các bản ghi liên quan trong bảng `Images`)
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool QuanAoExists(int id)
        {
            return _context.QuanAos.Any(e => e.Id == id);
        }
    }
}
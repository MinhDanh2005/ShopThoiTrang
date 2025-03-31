using System;
using System.Collections.Generic;

namespace ShopThoiTrang.Models
{
    public class QuanAo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
        public List<Image> Images { get; set; } = new List<Image>();
    }
}

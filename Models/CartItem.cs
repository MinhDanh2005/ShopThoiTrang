namespace ShopThoiTrang.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int QuanAoId { get; set; }
        public QuanAo QuanAo { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
    }
}
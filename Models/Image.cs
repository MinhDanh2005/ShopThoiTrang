using ShopThoiTrang.Models;

public class Image
{
    public int Id { get; set; }
    public string Url { get; set; }
    public int QuanAoId { get; set; }
    public QuanAo QuanAo { get; set; }
}
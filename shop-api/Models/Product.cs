namespace shop_api.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public short UnitInStock { get; set; }
        public decimal UnitPrice { get; set; }
    }
}

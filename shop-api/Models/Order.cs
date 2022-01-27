namespace shop_api.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}

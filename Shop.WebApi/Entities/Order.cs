namespace Shop.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime DateTime { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public IList<ProductOrder>? ProductOrders { get; set; }
    }
}

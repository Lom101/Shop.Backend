namespace Shop.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime DateTime { get; set; }
        public int UserId { get; set; }
    }
}

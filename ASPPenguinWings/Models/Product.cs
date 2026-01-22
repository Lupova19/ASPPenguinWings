namespace ASPPenguinWings.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public Category Categories { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string Apply { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public DateTime DateOn { get; set; }
        public ICollection<Order> Orders { get; set; }

    }
}

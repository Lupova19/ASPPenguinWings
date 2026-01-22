using System.Net.Http.Headers;

namespace ASPPenguinWings.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Products { get; set; }
        public string CustomerId { get; set; }
        public Customer Customers { get; set; }
        public int Quantity { get; set; }
        public DateTime DateOn { get; set; }
    }
}

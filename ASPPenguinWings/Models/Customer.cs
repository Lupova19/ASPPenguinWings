using Microsoft.AspNetCore.Identity;

namespace ASPPenguinWings.Models
{
    public class Customer : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOn { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}

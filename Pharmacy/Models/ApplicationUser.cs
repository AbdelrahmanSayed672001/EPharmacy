using Microsoft.AspNetCore.Identity;

namespace Pharmacy.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FName { get; set; }
        public string Address { get; set; }
    }
}

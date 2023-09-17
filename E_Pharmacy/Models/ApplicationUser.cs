using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace E_Pharmacy.Models
{
    public class ApplicationUser: IdentityUser
    {
        [DataType(DataType.PhoneNumber)]
        public int Phone { get; set; }
        public string Address { get; set; }
    }
}


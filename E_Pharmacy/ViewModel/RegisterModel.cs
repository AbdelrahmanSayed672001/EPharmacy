using System.ComponentModel.DataAnnotations;

namespace E_Pharmacy.Models
{
    public class RegisterModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}

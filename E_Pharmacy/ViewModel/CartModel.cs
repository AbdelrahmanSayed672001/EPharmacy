using E_Pharmacy.Models;
using System.ComponentModel.DataAnnotations;

namespace E_Pharmacy.ViewModel
{
    public class CartModel
    {
        public string Username{ get; set; }
        public string Medicine { get; set; }
        public int Quantity { get; set; }
    }
}

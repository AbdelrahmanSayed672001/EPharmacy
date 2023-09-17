using E_Pharmacy.Models;
using System.ComponentModel.DataAnnotations;

namespace E_Pharmacy.ViewModel
{
    public class MedicineViewModel
    {
        [MaxLength(100)]
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        public int medicineTypeId { get; set; }
    }
}

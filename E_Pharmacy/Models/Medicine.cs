using System.ComponentModel.DataAnnotations;

namespace E_Pharmacy.Models
{
    public class Medicine
    {
        public int Id { get; set; }
        [MaxLength (100)]
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        public int medicineTypeId{ get; set; }
        public MedicineType medicineType { get; set; }
    }
}

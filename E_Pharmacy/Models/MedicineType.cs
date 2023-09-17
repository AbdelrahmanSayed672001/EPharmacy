namespace E_Pharmacy.Models
{
    public class MedicineType
    {
        public int Id { get; set; }
        public string TypeName { get; set; }

        public List<Medicine> Medicines { get; set; }
    }
}

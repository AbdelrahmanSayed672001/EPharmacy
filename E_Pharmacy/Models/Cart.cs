namespace E_Pharmacy.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public Medicine medicine{ get; set; }
        public int Quantity{ get; set; }
        public double Price{ get; set; }
        public DateTime Date { get; set; }
    }
}

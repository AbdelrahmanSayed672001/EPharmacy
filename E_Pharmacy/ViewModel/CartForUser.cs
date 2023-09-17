namespace E_Pharmacy.ViewModel
{
    public class CartForUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Medicine { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public DateTime Date { get; set; }
    }
}

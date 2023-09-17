using E_Pharmacy.Models;
using E_Pharmacy.ViewModel;

namespace E_Pharmacy.Sefvices.CartServices
{
    public interface ICartService
    {
        Task<Cart> AddtoCartAsync(Cart model); 
        Task<IEnumerable<CartDetails>> GetAllAsync(); 
        Task<IEnumerable<CartForUser>> GetByUsername(string username); 
        Task<bool> isExisted(Cart model);
        Task<double> CalculateTotalPrice(string username);
        Task<string> Delete(string username);
    }
}

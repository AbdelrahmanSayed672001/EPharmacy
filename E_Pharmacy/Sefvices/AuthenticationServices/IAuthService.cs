using E_Pharmacy.Models;
using E_Pharmacy.ViewModel;

namespace E_Pharmacy.Sefvices.AuthenticationServices
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);
        Task<AuthModel> LoginAsync(LoginModel model);
        Task<string> AddRoleAsync(AddToRole model);
    }
}

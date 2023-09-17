using E_Pharmacy.Models;

namespace E_Pharmacy.Sefvices.ShortCommingServices
{
    public interface IShortComming
    {
        Task<ShortComming> Add(ShortComming shortComming);
        void Delete(ShortComming shortComming);
        Task<ShortComming> isExisted(string medicineName);
        Task<IEnumerable<ShortComming>> GetAll();
    }
}

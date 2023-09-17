using E_Pharmacy.Models;

namespace E_Pharmacy.Sefvices.MedicineServices
{
    public interface IMedicine
    {
        Task<Medicine> Add(Medicine medicine);
        Medicine Update(Medicine medicine);
        Task<Medicine> GetByName(string name);
        Task<IEnumerable<Medicine>> GetAll();
        Task<IEnumerable<Medicine>> GetPagination(int page, double pageSize);
        Task<Medicine> GetById(int id);
        void Delete(Medicine medicine);

    }
}

using E_Pharmacy.Models;

namespace E_Pharmacy.Sefvices.MedicineServices
{
    public interface IMedicineType: ICheckExistance
    {
        Task<MedicineType> Add(MedicineType medicineType);
        Task<IEnumerable<MedicineType>> GetAll();
    }
}

namespace E_Pharmacy.Sefvices.MedicineServices
{
    public interface ICheckExistance 
    {
        Task<bool> isExist(string name);

    }
}

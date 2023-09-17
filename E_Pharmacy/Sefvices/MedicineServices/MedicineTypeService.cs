using E_Pharmacy.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Pharmacy.Sefvices.MedicineServices
{
    public class MedicineTypeService : IMedicineType
    {
        private readonly ApplicationDBContext dBContext;

        public MedicineTypeService(ApplicationDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        public async Task<MedicineType> Add(MedicineType medicineType)
        {
            await dBContext.MedicineTypes.AddAsync(medicineType);
            await dBContext.SaveChangesAsync();
            return medicineType;
        }

        public async Task<IEnumerable<MedicineType>> GetAll()
        {
            return await dBContext.MedicineTypes.ToListAsync();
        }

        public async Task<bool> isExist(string name)
        {
            var result = await dBContext.MedicineTypes.FirstOrDefaultAsync(x=>x.TypeName.ToLower()==name.ToLower());

            if(result==null)
                // it means that name is not existed in table
                return false;

            // it means that name is already existed
            return true;
        }
    }
}

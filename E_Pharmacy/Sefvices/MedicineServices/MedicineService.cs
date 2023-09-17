using E_Pharmacy.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Pharmacy.Sefvices.MedicineServices
{
    public class MedicineService : IMedicine
    {
        private readonly ApplicationDBContext dBContext;

        public MedicineService(ApplicationDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<Medicine> Add(Medicine medicine)
        {
            await dBContext.Medicines.AddAsync(medicine);
            await dBContext.SaveChangesAsync();
            return medicine;
        }

        public void Delete(Medicine medicine)
        {
            dBContext.Medicines.Remove(medicine);
            dBContext.SaveChanges();

        }

        public async Task<IEnumerable<Medicine>> GetAll()
        {
            return await dBContext.Medicines.OrderBy(o => o.Name).ToListAsync();
        }
        
        public async Task<IEnumerable<Medicine>> GetPagination(int page,double pageSize)
        {
            //if(page == 1 )
            //   page=0;
            int size =Convert.ToInt32(pageSize);
            int totalnumber=(page-1)*size;
            var res= await dBContext.Medicines.Skip(totalnumber).Take(size).ToListAsync();
            return res;
        }

        public async Task<Medicine> GetById(int id)
        {
            return await dBContext.Medicines.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Medicine> GetByName(string name)
        {
            return await dBContext.Medicines.FirstOrDefaultAsync(x => x.Name == name);

        }
        public Medicine Update(Medicine medicine)
        {
            dBContext.Medicines.Update(medicine);
            dBContext.SaveChanges();
            return medicine;
        }
    }
}
